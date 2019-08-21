using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingExecuter
{
    public class LinuxPingCmd : SupPingCmd
    {
         public override string CmdFotmat { get; protected set; } = "{0}  -c {1}";
    }
}