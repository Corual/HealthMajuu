using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuApplications
{
    public interface IToolConfigApplication: IApplication
    {
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="newConfiguration"></param>
        /// <returns></returns>
        Task<JsonDataMsg<string>> UserAddConfigurationToToolAsync(ToolConfigDto newConfiguration);
      

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="newConfiguration"></param>
        /// <returns></returns>
          Task<object> UserAlterConfigurationToToolAsync(ToolConfigDto newConfiguration);

    }
}
