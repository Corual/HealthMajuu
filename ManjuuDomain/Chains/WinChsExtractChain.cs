using System;
using System.Text.RegularExpressions;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuDomain.Tools;

namespace ManjuuDomain.Chains
{
    /// <summary>
    /// 简体中文windows结果提取链
    /// </summary>
    public class WinChsExtractChain : IExtractable
    {
        public ExtractInfo Extract(string result)
        {
            ExtractInfo extInfo = new ExtractInfo();

            #region 情况一         
            // 正在 Ping 127.0.0.1 具有 32 字节的数据:
            // 来自 127.0.0.1 的回复: 字节=32 时间<1ms TTL=128
            // 来自 127.0.0.1 的回复: 字节=32 时间<1ms TTL=128
            // 来自 127.0.0.1 的回复: 字节=32 时间<1ms TTL=128
            // 来自 127.0.0.1 的回复: 字节=32 时间<1ms TTL=128

            // 127.0.0.1 的 Ping 统计信息:
            //     数据包: 已发送 = 4，已接收 = 4，丢失 = 0 (0% 丢失)，
            // 往返行程的估计时间(以毫秒为单位):
            //     最短 = 0ms，最长 = 0ms，平均 = 0ms
            #endregion

            #region 情况二
            //Ping 请求找不到主机 928.929.1.1。请检查该名称，然后重试。
            #endregion

            #region 情况三
            //选项 -d 不正确。
            //选项 -n 的值有错误，有效范围从 1 到 4294967295。
            #endregion

            #region 情况四
            // 正在 Ping 233.233.233.233 具有 32 字节的数据:
            // 请求超时。
            // 请求超时。
            // 请求超时。
            // 请求超时。
            // 233.233.233.233 的 Ping 统计信息:
            // 数据包: 已发送 = 4，已接收 = 0，丢失 = 4 (100% 丢失)，
            #endregion

            #region 情况五
            //'ping' 不是内部或外部命令
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
            if (Regex.IsMatch(lines[0], "选项(\\s)+([\\w\\W])+不正确"))
            {
                extInfo.InfoType = PingResultStatus.ExecuteError;
                return extInfo;
            }
            else if (Regex.IsMatch(lines[0], "选项(\\s)+([\\w\\W])+错误"))
            {
                extInfo.InfoType = PingResultStatus.ExecuteError;
                return extInfo;
            }
            else if (lines[0].Contains("不是内部或外部命令"))
            {
                extInfo.InfoType = PingResultStatus.PingNotfound;
                return extInfo;
            }
            #endregion

            #region 匹配第一行的ip
            //匹配第一行的ip
            Match ipMatch = Regex.Match(lines[0], UsualRegularExp.IPADRESS);
            if (!ipMatch.Success)
            {
                return ExtractInfo.ZeroInfo;
            }
            extInfo.IpV4 = ipMatch.Value;
            extInfo.Port = "80";
            #endregion

            #region 找不到主机
            if (Regex.IsMatch(lines[0], $"找不到主机(\\s+){extInfo.IpV4}"))
            {
                //访问不到ip地址
                extInfo.InfoType = PingResultStatus.HostNotfound;
                return extInfo;
            }
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
                if ("请求超时。" == currentLine)
                {
                    //超时只能初步判定为丢包，最后丢包率100%才是超时，无法访问
                    extInfo.InfoType = PingResultStatus.PacketLoss;
                }
                else if (currentLine.StartsWith(" 数据包") || currentLine.StartsWith("数据包"))
                {
                    //数据包: 已发送 = 4，已接收 = 0，丢失 = 4 (100% 丢失)，
                    Match lossRateMatch = Regex.Match(currentLine, "[\\s\\w\\W]+\\(([\\d.]+)%[\\s\\w\\W]+\\)");
                    if (lossRateMatch.Success)
                    {
                        string lossRate = lossRateMatch.Groups[1].Value;
                        //丢包100%,全部超时
                        if ("100" == lossRate)
                        {
                            extInfo.InfoType = PingResultStatus.Timeout;
                        }

                        extInfo.LossRate = "100" == lossRate ? 100 : Convert.ToInt32(lossRate);
                    }
                }
                else
                {
                    //     最短 = 0ms，最长 = 0ms，平均 = 0ms
                    Match responseTimeMatch = Regex.Match(currentLine, @"[^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms[,，][^\d]+=[^\d]+(\d+)ms");
                    if (!responseTimeMatch.Success)
                    {
                        //又有ip响应，又没超时，却没有响应时间统计信息，这个数据有问题，所以标记成无结果
                        //todao：需要加日志记录一下这个特殊情况
                        return ExtractInfo.ZeroInfo;
                    }

                    extInfo.InfoType = PingResultStatus.Pass;
                    extInfo.MinTime = Convert.ToInt32(responseTimeMatch.Groups[1]);
                    extInfo.Maxtime = Convert.ToInt32(responseTimeMatch.Groups[2]);
                    extInfo.AvgTime = Convert.ToInt32(responseTimeMatch.Groups[3]);
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