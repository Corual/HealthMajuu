using System;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using ManjuuDomain.Suppers;

namespace ManjuuDomain.HealthCheck
{
    public class CheckConfig : SupEntity, IAggregateRoot
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

        public static ToolRunParam GetRunParam(ToolConfigDto toolConfig)
        {
            ToolRunParam toolRunParam = new ToolRunParam();
            toolRunParam.WorkType = ((!string.IsNullOrWhiteSpace(toolConfig.StartToWrokTime)) &&
                (!string.IsNullOrWhiteSpace(toolConfig.StopToWorkTime))) ? ToolWorkType.TimeToTime : ToolWorkType.AllDay;

            toolRunParam.Timeout = toolConfig.PresetTimeout;
            toolRunParam.BreakTime = toolConfig.WorkSpan ?? 0;
            toolRunParam.TargetConnectionTimes = toolConfig.PingSendCount;



            if (ToolWorkType.TimeToTime == toolRunParam.WorkType)
            {
                toolRunParam.TimeToStart = toolConfig.StartToWrokTime;
                toolRunParam.TimeToStop = toolConfig.StopToWorkTime;
                int startHout = int.Parse(toolRunParam.TimeToStart.Substring(0, 2));
                int stopHout = int.Parse(toolRunParam.TimeToStop.Substring(0, 2));

                if (stopHout < startHout)
                {
                    //结束配置时间段，小于开始时间，则把结束时间加12小时
                    stopHout += 12;
                }
                else if (toolRunParam.TimeToStart == toolRunParam.TimeToStop)
                {
                    stopHout += 12;
                }

                toolRunParam.TimeToStop = $"{stopHout}:{toolRunParam.TimeToStart.Substring(3, 2)}";


            }

            return toolRunParam;

        }
    }
}