using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.DataPackages
{
    public abstract class BaseMsg
    {
        /// <summary>
        /// 业务结果
        /// </summary>
        public bool BusinessResult { get; set; }

        /// <summary>
        /// 失败消息
        /// </summary>
        public string FailMsg { get; set; }
    }
}
