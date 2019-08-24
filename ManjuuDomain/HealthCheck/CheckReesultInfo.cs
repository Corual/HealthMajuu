using ManjuuDomain.Chains;
using ManjuuDomain.ExtractInfos;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ManjuuDomain.HealthCheck
{
    public class CheckReesultInfo : SupEntity
    {
        #region 属性
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddresV4 { get; private set; }

        /// <summary>
        /// 目标端口号
        /// </summary>
        public string TargetPort { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }

        /// <summary>
        /// 结果状态
        /// </summary>
        public PingResultStatus Status { get; private set; }

        #region 响应时间,单位毫秒
        /// <summary>
        /// 平均响应时间
        /// </summary>
        public int AvgResponseTime { get; private set; }

        /// <summary>
        /// 最长响应时间
        /// </summary>
        public int MaxResponseTime { get; private set; }

        /// <summary>
        /// 最短响应时间
        /// </summary>
        public int MinResponseTime { get; private set; }
        #endregion

        /// <summary>
        /// 预设的超时判断基准时间
        /// </summary>
        public int PresetTimeout { get; private set; }

        /// <summary>
        /// 结果接收时间
        /// </summary>
        public DateTime ResultReceiveTime { get; private set; }

        /// <summary>
        /// 丢包率
        /// </summary>
        public int LossRate { get; private set; }
        #endregion

        #region 构造
        private CheckReesultInfo() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipv4">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="remarks">备注</param>
        /// <param name="status">状态</param>
        /// <param name="maxrep">最大响应时间</param>
        /// <param name="minrep">最小响应时间</param>
        /// <param name="avgrep">平均响应时间</param>
        /// <param name="presetTimeout">预设超时时间</param>
        /// <param name="receiveTime">结果接收时间</param>
        public CheckReesultInfo(string ipv4, string port, string remarks, PingResultStatus status, int maxrep, int minrep, int avgrep, int presetTimeout, DateTime receiveTime)
        {
            IpAddresV4 = ipv4;
            TargetPort = port;
            Remarks = remarks;
            Status = status;
            MaxResponseTime = maxrep;
            MinResponseTime = minrep;
            AvgResponseTime = avgrep;
            PresetTimeout = presetTimeout;
            ResultReceiveTime = receiveTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipv4">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="remarks">备注</param>
        /// <param name="status">状态</param>
        /// <param name="lossRate">丢包率</param>
        /// <param name="presetTimeout">预设超时时间</param>
        /// <param name="receiveTime">结果接收时间</param>
        public CheckReesultInfo(string ipv4, string port, string remarks, PingResultStatus status, int lossRate, int presetTimeout, DateTime receiveTime)
        {
            IpAddresV4 = ipv4;
            TargetPort = port;
            Remarks = remarks;
            Status = status;
            PresetTimeout = presetTimeout;
            ResultReceiveTime = receiveTime;
            LossRate = lossRate;
        }


        public CheckReesultInfo(string ipv4, string port, string remarks, PingResultStatus status, int maxrep, int minrep, int avgrep, int presetTimeout, DateTime receiveTime, int lossRate)
        {
            IpAddresV4 = ipv4;
            TargetPort = port;
            Remarks = remarks;
            Status = status;
            MaxResponseTime = maxrep;
            MinResponseTime = minrep;
            AvgResponseTime = avgrep;
            PresetTimeout = presetTimeout;
            ResultReceiveTime = receiveTime;
            LossRate = lossRate;
        }

        public CheckReesultInfo(string ipv4, string port, string remarks, PingResultStatus status, DateTime receiveTime)
        {
            IpAddresV4 = ipv4;
            TargetPort = port;
            Remarks = remarks;
            Status = status;
            ResultReceiveTime = receiveTime;
        }
        #endregion


        public static CheckReesultInfo ExtractionPingResult(string ipV4, string port, string remarks, DateTime receiveTime, string resultMessage)
        {
            //空串证明地址ping出了异常
            if (string.IsNullOrWhiteSpace(resultMessage))
            {
                //既然已经异常了，就退出当前任务执行，可以考虑统计同一个地址出错次数，一定时间内冻结任务，避免过多的访问不正常的地址
                return new CheckReesultInfo(ipV4, port, remarks, PingResultStatus.Exception, receiveTime);
            }

            try
            {
                return CheckReesultInfo.CreateFromExtractInfo(ipV4, port, remarks, receiveTime, ExtractChainFactory.GetExtractChain().Extract(resultMessage));
            }
            catch (System.Exception ex)
            {
                //todo:日志记录
                System.Console.WriteLine(ex.Message);

                return new CheckReesultInfo(ipV4, port, remarks, PingResultStatus.Exception, receiveTime);
            }

        }

        private static CheckReesultInfo CreateFromExtractInfo(string ipV4, string port, string remarks, DateTime receiveTime, ExtractInfo extractInfo)
        {


            Assembly convertAssembly = Assembly.GetAssembly(typeof(CheckReesultInfo));

            ConstructorInfo contInfo = convertAssembly.GetType($"ManjuuDomain.ExtractInfos.{extractInfo.InfoType.ToString()}ResultConverter")
            .GetConstructor(new Type[] { typeof(string), typeof(string), typeof(string), typeof(DateTime) });

            if (null == contInfo)
            {
                throw new EntryPointNotFoundException($"状态{extractInfo.InfoType.ToString()}没有对应的处理程序");
            }

            IResultConverter resultConverter = contInfo.Invoke(new object[] { ipV4, port, remarks, receiveTime }) as IResultConverter;

            if (null == resultConverter)
            {
                throw new NotSupportedException($"状态{extractInfo.InfoType.ToString()}无法使用IResultConverter处理");
            }

            return resultConverter.Convert(extractInfo);

        }

        public override string ToString()
        {
            if (Status == PingResultStatus.Exception)
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)},具体异常,请查看异常日志";
            }
            else if (Status == PingResultStatus.HostNotfound)
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)}";
            }
            else if (Status == PingResultStatus.CanNotAccess) 
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)},丢包率 = {LossRate}%";
            }
            else if (Status == PingResultStatus.PingNotfound)
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)},环境没有安装ping命令";
            }
            else if (Status == PingResultStatus.ExecuteError)
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)},又命令参数不正确，请查看具体异常日志";
            }
            else if (Status == PingResultStatus.NoneResult)
            {
                return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)}，需要排除并通知作者更新程序";
            }


            return $"[{IpAddresV4}]{Remarks}:{(PingResultStatusChs)((int)Status)},丢包率 = {LossRate}%,最短 = {MinResponseTime}ms,最长 = {MaxResponseTime}ms,平均 = {AvgResponseTime}ms,预设超时 = {PresetTimeout}ms";
        }

        public string GetResultInfoString()
        {
            return this.ToString();
        }

    }



}
