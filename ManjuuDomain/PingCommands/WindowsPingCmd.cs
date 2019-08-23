using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingCommands
{
    public class WindowsPingCmd : SupPingCmd
    {
        public override string RepeatParam { get; protected set; } = "-n";

        /// <summary>
        /// ��λ����
        /// </summary>
        public override string TimeoutParam { get; protected set; } = "-w";
    }
}