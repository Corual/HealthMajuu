using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.IpcService.ServiceContract
{
    public class DemoServiceContract : IDemoServiceContract
    {
        public float AddFloat(float x, float y)
        {
            Console.WriteLine("x + y = "+(x + y));
            return x + y;
        }
    }
}
