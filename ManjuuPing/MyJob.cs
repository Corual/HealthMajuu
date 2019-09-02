using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ManjuuPing
{
    internal class MyJob : IJob//创建IJob的实现类，并实现Excute方法。
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(("hello world=>")+ context.NextFireTimeUtc?.LocalDateTime);
            });
        }
    }
}