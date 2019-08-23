using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingCommands
{
    public class MacOXPingCmd:SupPingCmd
    {
        public override string RepeatParam { get; protected set; } = "";
        public override string TimeoutParam { get; protected set; } = "";
    }
}