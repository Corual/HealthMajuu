using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingCommands
{
    public class LinuxPingCmd : SupPingCmd
    {
        public override string RepeatParam { get; protected set; } = "-c";

        /// <summary>
        /// µ•Œª√Î
        /// </summary>
        public override string TimeoutParam { get; protected set; } = "-w";
    }
}