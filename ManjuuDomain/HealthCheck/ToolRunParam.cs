using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.HealthCheck
{
    /// <summary>
    /// 工具运行参数
    /// </summary>
    public class ToolRunParam
    {
        /// <summary>
        /// 运行的类型
        /// </summary>
        public ToolWorkType WorkType { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 休息时间
        /// </summary>
        public int BreakTime { get; set; }

        /// <summary>
        /// 目标尝试连接次数
        /// </summary>
        public int TargetConnectionTimes { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string TimeToStart { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        public string TimeToStop { get; set; }
    }
}
