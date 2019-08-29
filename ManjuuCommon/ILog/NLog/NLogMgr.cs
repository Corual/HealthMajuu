using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog.NLog
{
    public class NLogMgr
    {
        #region 留给不能IOC的地方用
        public static ICustomLog<ILogger> DefaultNLog { get; private set; } = new DefaultNLog();
        public static ICustomLog<ILogger> CheckLog { get; private set; } = new CheckLog();
        public static ICustomLog<ILogger> ExceptionLog { get; private set; } = new ExceptionLog();
        public static ICustomLog<ILogger> ProgramLog { get; private set; } = new ProgramLog();
        #endregion

        /// <summary>
        /// 对应配置文件中的 rules 的 logger
        /// </summary>
        public class LoggerName
        {
            /// <summary>
            /// 异常logger
            /// </summary>
            public const string Execption = "EXCEPTION_LOGGER";

            /// <summary>
            /// 检测目标 logger
            /// </summary>
            public const string Check = "CHECK_LOGGER";

            /// <summary>
            /// 程序 logger
            /// </summary>
            public const string Program = "PROGRAM_LOGGER";
        }

        /// <summary>
        /// 对应配置文件中的 <variable name="XXX" value="" />
        /// </summary>
        public enum ConfigurationVariables
        {
            /// <summary>
            /// 平台
            /// </summary>
            Terrace,
        }

        /// <summary>
        /// 对应配置文件中的 event-properties
        /// </summary>
        public enum EventProperties
        {
            /// <summary>
            /// 检测目标
            /// </summary>
            CheckTarget,

            /// <summary>
            /// 检测消息
            /// </summary>
            CheckMsg,

            /// <summary>
            /// 检测结果
            /// </summary>
            CheckResult,
        }


        public static void SetVariable(ConfigurationVariables variable, string value)
        {
            LogManager.Configuration.Variables[variable.ToString()] = value;
        }

        public static LogEventInfo GetEventInfo(LogLevel level, string msg = "", string loggerName = "", Exception ex = null)
        {
            LogEventInfo theEvent = new LogEventInfo(level, loggerName, msg);
            
            if (ex != null)
            {
                theEvent.Exception = ex;
            }

            return theEvent;
        }

        public static void SetEventProperties(LogEventInfo eventInfo, EventProperties property, string value)
        {
            eventInfo.Properties[property.ToString()] = value;
        }

        public static void SetEventProperties(LogEventInfo eventInfo, params SetEventPropertieParam[] param)
        {
            if (null == param || 0 == param.Length)
            {
                return;
            }

            foreach (var item in param)
            {
                SetEventProperties(eventInfo, item.Property, item.Value);
            }

        }


    }

    public class SetEventPropertieParam
    {
        public NLogMgr.EventProperties Property { get; set; }
        public string Value { get; set; }
    }
}
