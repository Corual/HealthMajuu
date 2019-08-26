using System.Runtime.InteropServices;
using ManjuuDomain.IDomain;
using ManjuuCommon.Tools;

namespace ManjuuDomain.Chains
{
    public sealed  class ExtractChainFactory
    {
        private volatile  static ExtractChainFactory _instance;
        private readonly static object _lock = new object();

        private IExtractable _chain = null;

        private ExtractChainFactory(){}

        private ExtractChainFactory(OSPlatform osplatform)
        {
             if (OSPlatform.Windows == osplatform)
            {
                WindowsExtractChain winChain = new WindowsExtractChain();
                winChain.AddExtractor(new WinEnExtractChain()).AddExtractor(new WinChsExtractChain());
                _chain = winChain;
            }
            else if (OSPlatform.Linux == osplatform)
            {
                _chain = null;
                throw new System.NotImplementedException();
            }
            else
            {
                _chain = null;
                throw new System.NotImplementedException();
            }
        }
        static ExtractChainFactory()
        {
            if(null == _instance){
                lock(_lock)
                {
                    if(null == _instance)
                    {
                        _instance = new ExtractChainFactory(OsPaltformMgr.GetInstance().Platform);
                       
                    }
                }
            }
        }

        public static IExtractable GetExtractChain()
        {
                return _instance._chain;
        }



    }
}