using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingCommands
{
    public class LinuxPingCmd : SupPingCmd
    {
        public override string RepeatParam { get; protected set; } = "-c";

        /// <summary>
        /// ��λ��
        /// </summary>
        public override string TimeoutParam { get; protected set; } = "-w";
    }
}