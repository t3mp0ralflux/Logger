using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Logger
{
    public static class Log
    {
        /// <summary>
        /// Writes to the console as information.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        public static void LogInfo(Type declaringType, string text)
        {
            var logger = LogManager.GetLogger(declaringType.FullName);
            logger.Info(text);
        }

        /// <summary>
        /// Logs an error to the log file.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        public static void LogError(Type declaringType, string text, Exception exception)
        {
            var logger = LogManager.GetLogger(declaringType.FullName);
            logger.Error(exception, text);
        }

        /// <summary>
        /// Logs debug messages to the logs.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        public static void LogDebug(Type declaringType, string text, Exception exception = null)
        {
            var logger = LogManager.GetLogger(declaringType.FullName);
            logger.Debug(exception, text);
        }

        static Log()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} | ${level} | ${message} | ${exception}"
            };
            config.AddTarget(consoleTarget);

            var errorTarget = new FileTarget("ErrorLog")
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${longdate} | ${level} | Message: ${message} | Exception: ${exception:format=ToString,Stacktrace}${newline}"
            };

            var infoTarget = new FileTarget("InfoTarget")
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${longdate} | ${level} | Message: ${message}"
            };

            var errorWrapper = new AsyncTargetWrapper(errorTarget, 5000, AsyncTargetWrapperOverflowAction.Discard);
            var infoWrapper = new AsyncTargetWrapper(infoTarget, 5000, AsyncTargetWrapperOverflowAction.Discard);

            config.AddTarget("ErrorWrapper", errorWrapper);
            config.AddTarget("InfoWrapper", infoWrapper);

            config.AddRuleForOneLevel(LogLevel.Info, "InfoWrapper");
            config.AddRuleForOneLevel(LogLevel.Error, "ErrorWrapper");
            config.AddRuleForOneLevel(LogLevel.Debug, "InfoWrapper");
            config.AddRuleForAllLevels(consoleTarget);

            LogManager.Configuration = config;
        }
    }
}