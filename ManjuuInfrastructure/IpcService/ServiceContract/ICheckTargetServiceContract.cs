using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.IpcService.ServiceContract
{
    public interface ICheckTargetServiceContract: IServiceContract
    {
        bool StopJob();

        void JobRestart();
    }
}
