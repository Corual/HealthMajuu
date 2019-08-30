using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.HealthCheck
{
    public enum ToolWorkType
    {
        /// <summary>
        /// 整天
        /// </summary>
        AllDay,

        /// <summary>
        /// 某个时间开始到某个时间结束
        /// </summary>
        TimeToTime,

        /// <summary>
        /// 定时
        /// </summary>
        Timing,
    }
}
