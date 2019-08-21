using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ManjuuDomain.HealthCheck;

namespace ManjuuPing
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //Task pingTask = PingCoreCode();

            new CheckTarget("www.baidu.com","80","百度").TryPingAsync();
            new CheckTarget("www.bilibili.com","80","哔哩哔哩").TryPingAsync();
            new CheckTarget("www.jianshu.com","80","简书").TryPingAsync();
            new CheckTarget("www.google.com","80","谷歌").TryPingAsync();
            Console.WriteLine("开始工作了");
            Console.ReadKey();
        }

        public static async Task PingCoreCode()
        {
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.FileName = "ping";
                    startInfo.Arguments = "www.baidu.com -c 4";
                    startInfo.RedirectStandardOutput = true;
                    //startInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.Start();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = await reader.ReadToEndAsync();
                        Console.WriteLine(result);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
