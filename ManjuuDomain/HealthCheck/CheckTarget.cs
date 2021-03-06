﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuCommon.Tools;
using ManjuuDomain.HealthCheckService;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;
using NLog;

namespace ManjuuDomain.HealthCheck
{
    public class CheckTarget : SupEntity, IPingable, IAggregateRoot
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

        private readonly IProgramLog<ILogger> _programLog = NLogMgr.ProgramNLog as IProgramLog<ILogger>;
        private readonly ICheckLog<ILogger> _checkLog = NLogMgr.CheckNLog as ICheckLog<ILogger>;

        #region 构造函数

        public CheckTarget(int id, string ipv4, string port, string remarks)
        {
            this.Id = id;
            this.IpAddresV4 = ipv4;
            this.TargetPort = port;
            this.Remarks = remarks;
        }
        #endregion


        /// <summary>
        /// 尝试异步ping目标地址
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <param name="tryCount">ping的次数</param>
        /// <returns></returns>
        public Task TryPingAsync(int timeout, int tryCount)
        {
            return Task.Run(async () =>
            {
                string pingResult = await CheckTargetService.PingRemoteTargetAsync(this, timeout, tryCount);

                Console.WriteLine(pingResult);

                //NLogMgr.DebugLog(_programLog, $"TryPingAsync=>{this.Remarks}调用结束{Environment.NewLine}");
                Console.WriteLine($"TryPingAsync=>{this.Remarks}调用结束{Environment.NewLine}");

                ////返回空串证明地址ping出了异常
                //if (string.IsNullOrWhiteSpace(pingResult))
                //{
                //    //既然已经异常了，就退出当前任务执行，可以考虑统计同一个地址出错次数，一定时间内冻结任务，避免过多的访问不正常的地址
                //    return;
                //}

                CheckReesultInfo checkReesultInfo = await CheckResultService.UnscramblePingResultAsync(IpAddresV4, TargetPort, Remarks, TimeMgr.GetLoaclDateTime(), pingResult);



                NLogMgr.DebugLog(_programLog, checkReesultInfo.GetResultInfoString());
                if (PingResultStatus.Pass == checkReesultInfo.Status)
                {
                    //NLogMgr.CheckMsgLog(_checkLog, LogLevel.Debug, checkReesultInfo.GetResultInfoString(), pingResult, $"[{this.IpAddresV4}]{this.Remarks}", checkReesultInfo.ResultReceiveTime);
                    return;
                }

                //todo:将非正常结果推送到消息队列(考虑开发时间问题,目前先直接写入日志)

                NLogMgr.CheckMsgLog(_checkLog, LogLevel.Error, checkReesultInfo.GetResultInfoString(), pingResult, $"[{this.IpAddresV4}]{this.Remarks}", checkReesultInfo.ResultReceiveTime);


                //Console.Clear();
            });
        }




    }
}
