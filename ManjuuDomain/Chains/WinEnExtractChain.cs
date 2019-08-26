using System;
using System.Text.RegularExpressions;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuCommon.Tools;

namespace ManjuuDomain.Chains
{
    public class WinEnExtractChain : IExtractable
    {
        public ExtractInfo Extract(string result)
        {
            ExtractInfo extInfo = new ExtractInfo();

            #region 情况一         
            // Pinging www.a.shifen.com [14.215.177.38] with 32 bytes of data:
            // Reply from 14.215.177.38: bytes=32 time=9ms TTL=128
            // Reply from 14.215.177.38: bytes=32 time=11ms TTL=128
            // Reply from 14.215.177.38: bytes=32 time=9ms TTL=128
            // Reply from 14.215.177.38: bytes=32 time=9ms TTL=128

            // Ping statistics for 14.215.177.38:
            //     Packets: Sent = 4, Received = 4, Lost = 0 (0% loss),
            // Approximate round trip times in milli-seconds:
            //     Minimum = 9ms, Maximum = 11ms, Average = 9ms
            #endregion

            #region 情况二
            //Ping request could not find host www.abc.bili. Please check the name and try again.
            #endregion

            #region 情况三
            //Bad option -c.
            //Bad value for option -n, valid range is from 1 to 4294967295.
            #endregion

            #region 情况四
            // Pinging 233.233.233.233 with 32 bytes of data:
            // Request timed out.
            // Request timed out.
            // Request timed out.
            // Request timed out.

            // Ping statistics for 233.233.233.233:
            //     Packets: Sent = 4, Received = 0, Lost = 4 (100% loss),
            #endregion

            #region 情况五
            //'pingg' is not recognized as an internal or external command,
            // operable program or batch file.
            #endregion

            //0：ip
            //2~？：过程回显数据
            //？：数据包统计信息
            //?:时间消耗统计
            string[] lines = result.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (0 == lines.Length)
            {
                return ExtractInfo.ZeroInfo;
            }

            #region 处理情况三，情况五
            //情况三，情况五莫得ip，先判断
            if (Regex.IsMatch(lines[0], "Bad(\\s)+option"))
            {
                extInfo.InfoType = PingResultStatus.ExecuteError;
                return extInfo;
            }
            else if (Regex.IsMatch(lines[0], "Bad(\\s)+([\\w\\W])+option"))
            {
                extInfo.InfoType = PingResultStatus.PingNotfound;
                return extInfo;
            }
            else if (lines[0].Contains("is not recognized as an internal or external command"))
            {
                extInfo.InfoType = PingResultStatus.PingNotfound;
                return extInfo;
            }
            #endregion

            #region 找不到主机
            if (Regex.IsMatch(lines[0], $"could not find host(\\s+){extInfo.IpV4}"))
            {
                //访问不到ip地址
                extInfo.InfoType = PingResultStatus.HostNotfound;
                return extInfo;
            }
            #endregion

            #region 匹配第一行的ip或域名
            //匹配第一行的ip
            Match ipMatch = Regex.Match(lines[0], UsualRegularExp.IPADRESS);
            //匹配第一行域名
            Match domainMatch = Regex.Match(lines[0],$"({UsualRegularExp.NET_DOMAIN})");

            //ip或域名都不匹配,只能推出解析
            if (!(ipMatch.Success || domainMatch.Success ))
            {
                return ExtractInfo.ZeroInfo;
            }
            
            extInfo.IpV4 = ipMatch.Success?ipMatch.Value:domainMatch.Groups[1].Value;
            extInfo.Port = "80";
            #endregion



            #region 剩下的参数循环解析
            for (int i = 1; i < lines.Length; i++)
            {
                string currentLine = lines[i];
                //行信息里含有ip证明还不是我们想要的数据
                if (currentLine.Contains(extInfo.IpV4))
                {
                    continue;
                }

                //情况四简单，先处理
                if ("Request timed out." == currentLine)
                {
                    //超时只能初步判定为丢包，最后丢包率100%才是超时，无法访问
                    extInfo.InfoType = PingResultStatus.PacketLoss;
                    continue;
                }
                else if (currentLine.Contains("Packets"))
                {
                    //数据包: 已发送 = 4，已接收 = 0，丢失 = 4 (100% 丢失)，
                    Match lossRateMatch = Regex.Match(currentLine, "[\\s\\w\\W]+\\(([\\d.]+)%[\\s\\w\\W]+\\)");
                    if (lossRateMatch.Success)
                    {
                        string lossRate = lossRateMatch.Groups[1].Value;
                        //丢包100%,全部超时
                        if ("100" == lossRate)
                        {
                            extInfo.InfoType = PingResultStatus.CanNotAccess;
                        }

                        extInfo.LossRate = "100" == lossRate ? 100 : Convert.ToInt32(lossRate);

                        //如果是最后一行，需要再这里返回，否则出了循环，就会变成了return ExtractInfo.ZeroInfo;
                        if (i == (lines.Length - 1))
                        {
                            return extInfo;
                        }
                    }
                }
                else if (currentLine.Contains("Minimum = "))
                {
                    //     Minimum = 9ms, Maximum = 11ms, Average = 9ms
                    Match responseTimeMatch = Regex.Match(currentLine, @"[^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms");
                    if (!responseTimeMatch.Success)
                    {
                        //又有ip响应，又没超时，却没有响应时间统计信息，这个数据有问题，所以标记成无结果
                        //todao：需要加日志记录一下这个特殊情况
                        return ExtractInfo.ZeroInfo;
                    }


                    extInfo.MinTime = Convert.ToInt32(responseTimeMatch.Groups[1].Value);
                    extInfo.Maxtime = Convert.ToInt32(responseTimeMatch.Groups[2].Value);
                    extInfo.AvgTime = Convert.ToInt32(responseTimeMatch.Groups[3].Value);

                    //如果之前没有丢过包，才将他设置为通过
                    if(PingResultStatus.PacketLoss != extInfo.InfoType)
                    {
                        extInfo.InfoType = PingResultStatus.Pass; 
                    }
                    //todo:获取系统设置的预设超时时间
                    int presetTime = 1000;
                    extInfo.InfoType = extInfo.AvgTime <= presetTime ? extInfo.InfoType : PingResultStatus.Timeout;

                    return extInfo;

                }





            }
            #endregion

            //出了循环还没又返回，证明前面都么有合适的值可以填，给结果类型重置成为none
            //todo:日志记录这个特殊情况
            return ExtractInfo.ZeroInfo;
        }
    }
}