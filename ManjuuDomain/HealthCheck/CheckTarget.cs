using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManjuuDomain.HealthCheckService;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;
namespace ManjuuDomain.HealthCheck
{
    public class CheckTarget : SupEntity, IPingable
    {
        #region 属性
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddresV4 { get; private set; }

        /// <summary>
        /// 目标端口号
        /// </summary>
        public string TargetPort { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }
        #endregion


        #region 构造函数
        public CheckTarget()
        {

        }

        public CheckTarget(string ipv4, string port, string remarks)
        {
            this.IpAddresV4 = ipv4;
            this.TargetPort = port;
            this.Remarks = remarks;
        }
        #endregion


        /// <summary>
        /// 尝试异步ping目标地址
        /// </summary>
        /// <returns></returns>
        public Task TryPingAsync()
        {
            return Task.Run(async () =>
            {
                string pingResult = await CheckTargetService.PingRemoteTargetAsync(this);
                Console.WriteLine($"TryPingAsync=>{this.Remarks}调用结束");

                //返回空串证明地址ping出了异常
                if (string.IsNullOrWhiteSpace(pingResult))
                {
                    //既然已经异常了，就退出当前任务执行，可以考虑统计同一个地址出错次数，一定时间内冻结任务，避免过多的访问不正常的地址
                    return;
                }


            //    await UnscramblePingResultAsync(pingResult);


            });
        }

        /// <summary>
        /// 解析Ping执行结果
        /// </summary>
        /// <param name="result">ping执行结果</param>
        // private  Task UnscramblePingResultAsync(string result)
        // {

        // }


    }
}
