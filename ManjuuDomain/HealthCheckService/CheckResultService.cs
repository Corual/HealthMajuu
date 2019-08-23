using ManjuuDomain.HealthCheck;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuDomain.HealthCheckService
{
   public static class CheckResultService
    {
        /// <summary>
        /// 异步解析Ping执行结果
        /// </summary>
        /// <param name="result">ping执行结果</param>
        public static Task<CheckReesultInfo> UnscramblePingResultAsync(string ipV4, string port, string remarks,  DateTime receiveTime,string result)
        {
            return Task.Run(()=> {
                 return CheckReesultInfo.ExtractionPingResult(ipV4, port, remarks, receiveTime,result);
            });
        }
    }
}
