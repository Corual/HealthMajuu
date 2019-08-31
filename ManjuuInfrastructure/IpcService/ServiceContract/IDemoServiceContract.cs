using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.IpcService.ServiceContract
{
    public interface IDemoServiceContract: IServiceContract
    {
         float AddFloat(float x, float y);
    }
}
