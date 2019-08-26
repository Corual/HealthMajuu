
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.Mvc
{
   public class HappyJsonResult : JsonResult
    {


        public HappyJsonResult(object value) : base(value)
        {
           
        }

        public HappyJsonResult(object value, JsonSerializerSettings serializerSettings) : base(value, serializerSettings)
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //忽略类型循环引用问题
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //json中属性开头字母小写的驼峰命名
            serializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //设置时间类型序列化字符串的格式
        }
    }
}
