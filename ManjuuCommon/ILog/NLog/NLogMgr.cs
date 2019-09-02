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
        public static ICustomLog<ILogger> CheckNLog { get; private set; } = new CheckLog();
        public static ICustomLog<ILogger> ExceptionNLog { get; private set; } = new ExceptionLog();
        public static ICustomLog<ILogger> ProgramNLog { get; private set; } = new ProgramLog();
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

            /// <summary>
            /// 接收时间
            /// </summary>
            CheckReceive,
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="msg"></param>
        public static void DebugLog(ICustomLog<ILogger> log, string msg)
        {
            if (null == log) { return; }

            log.GetLogger().Debug(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="msg"></param>
        public static void InfoLog(ICustomLog<ILogger> log, string msg)
        {
            if (null == log) { return; }

            log.GetLogger().Info(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="msg"></param>
        public static void TraceLog(ICustomLog<ILogger> log, string msg)
        {
            if (null == log) { return; }

            log.GetLogger().Trace(msg);
        }

        /// <summary>
        /// 错误异常日志
        /// </summary>
        /// <param name="exceptionLog"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void ErrorExLog(ICustomLog<ILogger> exceptionLog, string msg, Exception ex)
        {
            if (null == exceptionLog) { return; }

            exceptionLog.GetLogger().Error(ex, msg);
        }

        /// <summary>
        /// 错误异常日志
        /// </summary>
        /// <param name="fatalLog"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void FatalLog(ICustomLog<ILogger> fatalLog, string msg, Exception ex)
        {
            if (null == fatalLog) { return; }

            fatalLog.GetLogger().Fatal(ex, msg);
        }

        /// <summary>
        /// 检测结果日志
        /// </summary>
        /// <param name="checkLog"></param>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <param name="equipment"></param>
        public static void CheckMsgLog(ICheckLog<ILogger> checkLog, LogLevel level, string msg, string result, string equipment, DateTime receiveTime)
        {
            LogEventInfo theEvent = NLogMgr.GetEventInfo(level, "", NLogMgr.LoggerName.Check);
            NLogMgr.SetEventProperties(theEvent,
                new SetEventPropertieParam(NLogMgr.EventProperties.CheckTarget,  equipment),
                new SetEventPropertieParam(NLogMgr.EventProperties.CheckMsg, msg ),
                new SetEventPropertieParam(NLogMgr.EventProperties.CheckResult, result),
                new SetEventPropertieParam(NLogMgr.EventProperties.CheckReceive, receiveTime.ToString("yyyy-MM-dd HH:mm:ss.ffff")));

            checkLog.GetLogger().Log(theEvent);
        }

    }

    public class SetEventPropertieParam
    {
        public NLogMgr.EventProperties Property { get; set; }
        public string Value { get; set; }

        public SetEventPropertieParam()
        {

        }

        public SetEventPropertieParam(NLogMgr.EventProperties property, string value)
        {
            Property = property;
            Value = value;
        }
    }
}
