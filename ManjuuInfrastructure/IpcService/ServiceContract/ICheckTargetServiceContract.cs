using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuInfrastructure.IpcService.ServiceContract
{
    public interface ICheckTargetServiceContract: IServiceContract
    {
        Task<bool> StopJob();

        Task JobRestart();
    }
}
