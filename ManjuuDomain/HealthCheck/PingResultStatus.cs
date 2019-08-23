namespace ManjuuDomain.HealthCheck
{
    public enum PingResultStatus
    {
        /// <summary>
        /// 正常通过
        /// </summary>
        Pass,

        /// <summary>
        /// 超时
        /// </summary>
        Timeout,

        /// <summary>
        /// 主机地址解析不了
        /// </summary>
        HostNotfound,

        /// <summary>
        /// 没有Ping命令
        /// </summary>
        PingNotfound,

        /// <summary>
        /// 检测发生异常
        /// </summary>
        Exception,

        /// <summary>
        /// 丢包
        /// </summary>
        PacketLoss,

        /// <summary>
        /// 命令执行错误
        /// </summary>
        ExecuteError,

        /// <summary>
        /// 没有解析结果
        /// </summary>
        NoneResult, 
    }

}