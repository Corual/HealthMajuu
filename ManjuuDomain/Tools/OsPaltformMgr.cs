using System;
using System.Runtime.InteropServices;

namespace ManjuuDomain.Tools
{
    public class OsPaltformMgr
    {
        
        private  static volatile  OsPaltformMgr _instance = new OsPaltformMgr();
        public OSPlatform Platform { get; private set; }

        static OsPaltformMgr()
        {
            //判断程序运行平台
           _instance.Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows :
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OSPlatform.Linux : OSPlatform.OSX;
        }
        
        public static OsPaltformMgr GetInstance()
        {
            return _instance;            
        }
        
        
    }
}