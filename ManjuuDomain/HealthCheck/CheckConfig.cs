using System;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.HealthCheck
{
    public class CheckConfig : SupEntity,IAggregateRoot
    {
        /// <summary>
        /// 开始工作时间
        /// </summary>
        /// <value></value>
        public DateTime? StartToWrokTime { get; private set; }

        /// <summary>
        /// 停止工作时间
        /// </summary>
        /// <value></value>
        public DateTime? StopToWorkTime { get; private set; }

        /// <summary>
        ///每轮工作间隔时间，单位秒
        /// </summary>
        /// <value></value>
        public int? WorkSpan { get; private set; }

        /// <summary>
        /// 预设超时时间,单位毫秒
        /// </summary>
        /// <value></value>
        public int PresetTimeout { get; private set; }

        /// <summary>
        /// 每个地址ping的次数
        /// </summary>
        /// <value></value>
        public int PingSendCount { get; private set; }


        public CheckConfig(int id, DateTime? startToWork, DateTime? endToWork, int? workSpan, int presetTimeout, int pingSendCount)
        {
            this.Id = id;
            this.StartToWrokTime = startToWork;
            this.StopToWorkTime = endToWork;
            this.WorkSpan = workSpan;
            this.PresetTimeout = presetTimeout;
            this.PingSendCount = pingSendCount;
        }
    }
}