using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManjuuPing
{
    [DisallowConcurrentExecution]
    public class PingJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            Console.WriteLine("Next time => "+context.NextFireTimeUtc?.LocalDateTime);
            
            var runData = context.JobDetail.JobDataMap["runData"] as PingHostedService;

            if (null == runData) { return; }

            Console.WriteLine("Start============"+ runData.RunParam.TimeToStart);

            do
            {
                var list = await runData.GetDataPage(runData.CurrentPage);
                if (null == list || 0 == list.Count)
                {
                    if (1 >= runData.TotalPage)
                    {
                        //没数据，但是只有1页，可以直接退出
                        return;
                    }
                    else
                    {
                        //没数据,有多页,该页数据跳过，执行下一页数据
                        continue;
                    }
                }

                foreach (var item in list)
                {
                    item.TryPingAsync(runData.RunParam.Timeout, runData.RunParam.TargetConnectionTimes);
                }


            } while (++runData.CurrentPage <= runData.TotalPage);

            if (runData.CurrentPage > runData.TotalPage)
            {
                //每轮把所有目标执行完毕都把页数重置成1
                runData.CurrentPage = 1;
            }

            if (runData.RunParam.BreakTime > 0)
            {
                Thread.Sleep(runData.RunParam.BreakTime);
            }
        }
    }
}
