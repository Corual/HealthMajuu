using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ManjuuPing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PingCoreCode();
            Console.ReadLine();
        }

        public static async void PingCoreCode()
        {
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.FileName = "ping";
                    startInfo.Arguments = "www.baidu.com";
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
