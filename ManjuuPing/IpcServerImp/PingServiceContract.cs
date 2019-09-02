using ManjuuInfrastructure.IpcService.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuPing.IpcServerImp
{
    public class PingServiceContract : ICheckConfigServiceContract, ICheckTargetServiceContract
    {
        /// <summary>
        /// 配置更改响应事件
        /// </summary>
        public static event Func<Task> ResponseConfigModified;

        /// <summary>
        /// 响应停止作业事件
        /// </summary>
        public static event Func<Task<bool>> ResponseStopJob;

        /// <summary>
        /// 响应重新作业事件
        /// </summary>
        public static event Func<Task> ResponseJobRestart;

        public Task ConfigWasModify()
        {
            if (null != ResponseConfigModified)
            {
              return  ResponseConfigModified();
            }

            return Task.CompletedTask;
        }

        public Task JobRestart()
        {
            if (null != ResponseJobRestart)
            {
               return ResponseJobRestart();
            }

            return Task.CompletedTask;
        }

        public Task<bool> StopJob()
        {
            if (null != ResponseStopJob)
            {
                return ResponseStopJob();
            }

            return Task.FromResult<bool>(false);
        }
    }
}
