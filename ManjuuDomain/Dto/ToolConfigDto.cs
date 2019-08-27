using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuDomain.Dto
{
    public class ToolConfigDto
    {
        /// <summary>
        /// 实体的id
        /// </summary>
        public int Id { get;  set; }

        /// <summary>
        /// 开始工作时间
        /// </summary>
        /// <value></value>
        public string StartToWrokTime { get;  set; }

        /// <summary>
        /// 停止工作时间
        /// </summary>
        /// <value></value>
        public string StopToWorkTime { get;  set; }

        /// <summary>
        ///每轮工作间隔时间，单位秒
        /// </summary>
        /// <value></value>
        public int? WorkSpan { get;  set; }

        /// <summary>
        /// 预设超时时间,单位毫秒
        /// </summary>
        /// <value></value>
        public int PresetTimeout { get;  set; }

        /// <summary>
        /// 每个地址ping的次数
        /// </summary>
        /// <value></value>
        public int PingSendCount { get;  set; }



    }
}
