using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.DataPackages
{
    public class JsonDataMsg<T>: BaseMsg
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public T ResponseData { get; set; }

        public JsonDataMsg()
        {
            
        }

        public JsonDataMsg(T data, bool success, string msg)
        {
            this.ResponseData = data;
            this.BusinessResult = success;
            this.Msg = msg;
        }
   
    }
}
