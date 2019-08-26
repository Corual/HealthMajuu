using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;

namespace ManjuuInfrastructure.Log {
    public class EFCoreConsoleLog : ILogger {
        private readonly string _categoryName;
        public EFCoreConsoleLog (string categoryName) {
            _categoryName = categoryName;
        }
        public IDisposable BeginScope<TState> (TState state) {
            //throw new NotImplementedException();
            //简单调试记录，这个用不到
            return NullScope.Instance;
        }

        public bool IsEnabled (LogLevel logLevel) {
            //throw new NotImplementedException();
            //这里判断日志等级，控制哪些日志等级做记录
            return LogLevel.Information == logLevel;

        }

        public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            //throw new NotImplementedException();
            //ef core执行数据库命令时的categoryName为Microsoft.EntityFrameworkCore.Database.Command
            //日志级别为Information
            if ("Microsoft.EntityFrameworkCore.Database.Command" != _categoryName) {
                return;
            }

            //获取格式化后的内容
            string logFormatterContent = formatter (state, exception);

            //开始向控制台打印日志
            Console.WriteLine ();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine ($"============Begin->{nameof(EFCoreConsoleLog)}============");
            Console.WriteLine (logFormatterContent);
            Console.WriteLine ($"============End->{nameof(EFCoreConsoleLog)}============");
            Console.ResetColor ();

        }
    }
}