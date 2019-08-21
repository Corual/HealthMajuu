using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingExecuter
{
    public class WindowsPingCmd:SupPingCmd
    {
        public override string CmdFotmat { get; protected set; } = "{0}  -n {1}";
    }
}