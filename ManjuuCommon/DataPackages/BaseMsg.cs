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
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
