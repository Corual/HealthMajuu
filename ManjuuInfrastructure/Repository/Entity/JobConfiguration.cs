using System;

namespace ManjuuInfrastructure.Repository.Entity
{
    public class JobConfiguration : BaseEntity
    {
        /// <summary>
        /// 开始工作时间
        /// </summary>
        /// <value></value>
        public DateTime? StartToWrokTime { get; set; }

        /// <summary>
        /// 停止工作时间
        /// </summary>
        /// <value></value>
        public DateTime? StopToWrokTime { get; set; }

        /// <summary>
        ///每轮工作间隔时间，单位秒
        /// </summary>
        /// <value></value>
        public int? WorkSpan { get; set; }

        /// <summary>
        /// 预设超时时间,单位毫秒
        /// </summary>
        /// <value></value>
        public int PresetTimeout { get; set; }

        /// <summary>
        /// 每个地址ping的次数
        /// </summary>
        /// <value></value>
        public int PingSendCount { get; set; }
    }
}