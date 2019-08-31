using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.IpcService.ServiceContract
{
   public  interface ICheckConfigServiceContract: IServiceContract
    {
        /// <summary>
        /// 配置已更改
        /// </summary>
        /// <returns></returns>
        void ConfigWasModify();
    }
}
