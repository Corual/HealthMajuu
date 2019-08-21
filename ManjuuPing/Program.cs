using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuPing
{
    class Program
    {
        static void Main(string[] args)
        {
            //判断程序运行平台
            OSPlatform osP = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)?OSPlatform.Windows:
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux)?OSPlatform.Linux:OSPlatform.OSX;
            Console.WriteLine($"woking in {osP}");

            Task pingTask = PingCoreCode();
            Console.WriteLine("开始工作了");
            
            if(OSPlatform.Windows == osP)
            {
                Console.ReadLine();
            }else
            {
                pingTask.Wait();
            }
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
