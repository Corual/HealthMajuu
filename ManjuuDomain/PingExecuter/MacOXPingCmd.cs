using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingExecuter
{
    public class MacOXPingCmd:SupPingCmd
    {
         public override string CmdFotmat { get; protected set; } = "{0}";
    }
}