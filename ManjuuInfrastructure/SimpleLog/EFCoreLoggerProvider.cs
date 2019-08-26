using Microsoft.Extensions.Logging;

namespace ManjuuInfrastructure.Log {
    /// <summary>
    /// 自定义日志提供器
    /// </summary>
    public class EFCoreLoggerProvider : ILoggerProvider {
        public ILogger CreateLogger (string categoryName) {
            //这里需要返回一个实现了ILogger的log实现
            //throw new System.NotImplementedException();
            return new EFCoreConsoleLog (categoryName);
        }

        public void Dispose () { }
    }
}