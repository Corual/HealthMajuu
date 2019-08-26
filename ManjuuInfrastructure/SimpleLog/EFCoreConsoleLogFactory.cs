using Microsoft.Extensions.Logging;

namespace ManjuuInfrastructure.Log {
    public class EFCoreConsoleLogFactory {

        public static ILoggerFactory FactoryInstance { get; private set; } = new LoggerFactory (new ILoggerProvider[] { new EFCoreLoggerProvider () });

        private EFCoreConsoleLogFactory () {

        }

    }
}