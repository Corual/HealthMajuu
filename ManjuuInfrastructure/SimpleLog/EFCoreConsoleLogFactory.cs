using Microsoft.Extensions.Logging;

namespace ManjuuInfrastructure.Log {
    public class EFCoreConsoleLogFactory {

        public  ILoggerFactory FactoryInstance { get; private set; } = new LoggerFactory (new ILoggerProvider[] { new EFCoreLoggerProvider () });

        public EFCoreConsoleLogFactory () {

        }

    }
}