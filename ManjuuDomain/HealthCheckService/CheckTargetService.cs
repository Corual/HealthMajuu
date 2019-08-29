using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ManjuuDomain.IDomain;
using ManjuuDomain.PingCommands;
using ManjuuDomain.Suppers;
using ManjuuCommon.Tools;
using System.Collections.Generic;
using ManjuuDomain.Dto;

namespace ManjuuDomain.HealthCheckService
{
    public static class CheckTargetService
    {
        /// <summary>
        /// 操作系统平台
        /// </summary>
        private static OSPlatform _platform;

        /// <summary>
        /// Ping命令
        /// </summary>
        private static SupPingCmd _pingCmd;

        static CheckTargetService()
        {
            
            _platform = OsPaltformMgr.GetInstance().Platform;

            System.Console.WriteLine($"wroking in {_platform}");

            _pingCmd = new UniformPingFactory(_platform).PingCmd;
        }

        /// <summary>
        /// 异步Ping远程目标
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>方法执行成功返回非空且非空白字符串，否则统一返回空串</returns>
        public static async Task<string> PingRemoteTargetAsync(IPingable target)
        {
            string result = string.Empty;
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.FileName = _pingCmd.PingName;
                    //对目标执行ping操作四次,超时为1秒
                    //todao:这里的ping4次数跟超时预设，需要使用数据库配置
                    startInfo.Arguments = string.Format($" {target.IpAddresV4} {_pingCmd.RepeatParam} 4 {_pingCmd.TimeoutParam} 1000");


                    //重定向输出流，便于获取ping命令结果
                    startInfo.RedirectStandardOutput = true;
                    //startInfo.StandardOutputEncoding = Encoding.UTF8;

                    Console.WriteLine($"执行命令:{_pingCmd.PingName} {startInfo.Arguments}{Environment.NewLine}");
                    //开始执行命令
                    process.Start();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = await reader.ReadToEndAsync();
                        System.Console.WriteLine($"{target.Remarks}执行完成===={TimeMgr.GetLoaclDateTime().ToString("yyyy-MM-dd HH:mm:ss:ffff")}===={Environment.NewLine}");
                        Console.WriteLine(result);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

    }
}