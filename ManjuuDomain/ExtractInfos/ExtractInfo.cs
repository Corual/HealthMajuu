using ManjuuDomain.HealthCheck;

namespace ManjuuDomain.ExtractInfos
{
    public struct ExtractInfo
    {
        public static ExtractInfo ZeroInfo { get; private set; } = new ExtractInfo(){InfoType = PingResultStatus.NoneResult, IpV4=string.Empty, Port=string.Empty};

        /// <summary>
        /// 地址
        /// </summary>
        public string IpV4 { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 丢包率
        /// </summary>
        public int LossRate { get; set; }

        /// <summary>
        /// 最长响应时间
        /// </summary>
        public int Maxtime { get; set; }

        /// <summary>
        /// 最短响应时间
        /// </summary>
        public int MinTime { get; set; }

        /// <summary>
        /// 平均响应时间
        /// </summary>
        public int AvgTime { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public PingResultStatus InfoType { get; set; }


        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return $"{IpV4}{Port}{InfoType}".GetHashCode();
        }


        public static bool operator ==(ExtractInfo left, ExtractInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExtractInfo left, ExtractInfo right)
        {
            return !left.Equals(right);
        }

    }
}