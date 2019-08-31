using ManjuuInfrastructure.IpcService.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuPing.IpcServerImp
{
    public class PingServiceContract : ICheckConfigServiceContract, ICheckTargetServiceContract
    {
        /// <summary>
        /// 配置更改响应事件
        /// </summary>
        public static event Action ResponseConfigModified;

        /// <summary>
        /// 响应停止作业事件
        /// </summary>
        public static event Func<bool> ResponseStopJob;

        /// <summary>
        /// 响应重新作业事件
        /// </summary>
        public static event Action ResponseJobRestart;

        public void ConfigWasModify()
        {
            if (null != ResponseConfigModified)
            {
                ResponseConfigModified();
            }
        }

        public void JobRestart()
        {
            if (null != ResponseJobRestart)
            {
                ResponseJobRestart();
            }
        }

        public bool StopJob()
        {
            if (null != ResponseStopJob)
            {
                return ResponseStopJob();
            }

            return false;
        }
    }
}
