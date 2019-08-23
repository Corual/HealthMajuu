namespace ManjuuDomain.HealthCheck
{
    public enum PingResultStatusChs
    {
        /// <summary>
        /// Pass
        /// </summary>
        正常通过,

        /// <summary>
        /// Timeout
        /// </summary>
        超时,

        /// <summary>
        /// HostNotfound
        /// </summary>
        主机地址解析不了,

        /// <summary>
        /// PingNotfound
        /// </summary>
        没有Ping命令,

        /// <summary>
        /// Exception
        /// </summary>
        检测发生异常,

        /// <summary>
        /// PacketLoss
        /// </summary>
        丢包,

        /// <summary>
        /// ExecuteError
        /// </summary>
        命令执行错误,

        /// <summary>
        /// NoneResult
        /// </summary>
        没有解析结果,
    }
}