using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ManjuuDomain.IDomain;
using ManjuuDomain.PingExecuter;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.HealthCheckService
{
    public static class CheckTargetService
    {
        private static OSPlatform _platform;

        private static SupPingCmd _pingCmd;

        static CheckTargetService()
        {
            //判断程序运行平台
            _platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows :
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OSPlatform.Linux : OSPlatform.OSX;

            System.Console.WriteLine($"wroking in {_platform}");


            _pingCmd = new UniformPingFactory(_platform).PingCmd;
        }

        public static async Task PingRemoteTargetAsync(IPingable target)
        {

            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.FileName = _pingCmd.PingName;
                    //对目标执行ping操作四次
                   startInfo.Arguments = string.Format(_pingCmd.CmdFotmat,target.IpAddresV4,4.ToString());


                    //重定向输出流，便于获取ping命令结果
                    startInfo.RedirectStandardOutput = true;
                    //startInfo.StandardOutputEncoding = Encoding.UTF8;
                    //开始执行命令
                    process.Start();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = await reader.ReadToEndAsync();
                        System.Console.WriteLine($"{target.Remarks}执行完成===={DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:ffff")}====");
                        Console.WriteLine(result);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

    }
}