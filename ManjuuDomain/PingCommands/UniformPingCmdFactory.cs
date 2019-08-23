using System.Runtime.InteropServices;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.PingCommands
{
    /// <summary>
    /// 统一的ping命令工厂
    /// </summary>
    public sealed class UniformPingFactory
    {
        /// <summary>
        ///  具体的调用命令
        /// </summary>
        /// <value></value>
        public SupPingCmd PingCmd { get; private set; }


        public UniformPingFactory(OSPlatform platform)
        {
            if (OSPlatform.Windows == platform)
            {
                PingCmd = new WindowsPingCmd();
            }
            else if (OSPlatform.Linux == platform)
            {
                PingCmd = new LinuxPingCmd();
            }
            else
            {
                PingCmd = new MacOXPingCmd();
            }
        }

    }
}