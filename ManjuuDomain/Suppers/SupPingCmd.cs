namespace ManjuuDomain.Suppers
{
    /// <summary>
    /// 平台ping命令抽象
    /// </summary>
    public abstract class SupPingCmd
    {
        /// <summary>
        /// 具体实现ping命令的执行程序
        /// </summary>
        /// <value></value>
        public virtual string PingName{get; protected set;} = "ping";

        /// <summary>
        /// 重复参数
        /// </summary>
        public abstract string RepeatParam { get; protected set; }


        /// <summary>
        /// 超时参数
        /// </summary>
        public abstract string TimeoutParam { get; protected set; }

    }
}