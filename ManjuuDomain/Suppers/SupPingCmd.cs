namespace ManjuuDomain.Suppers
{
    public abstract class SupPingCmd
    {
        /// <summary>
        /// 具体实现ping命令的执行程序
        /// </summary>
        /// <value></value>
        public virtual string PingName{get; protected set;} = "ping";

        /// <summary>
        /// 命令格式
        /// </summary>
        /// <value></value>
        public abstract string CmdFotmat{get; protected set;}

    }
}