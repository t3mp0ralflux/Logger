using System;
using NLog;
using NLog.Config;
using NLog.Targets;

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
        public static void LogInfo(Type declaringType, string text, Exception exception = null)
        {
            var logger = LogManager.GetLogger(declaringType.FullName);
            logger.Info(exception,text);
        }

        /// <summary>
        /// Logs an error to the log file.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        public static void LogError(Type declaringType, string text, Exception exception = null)
        {
            var logger = LogManager.GetLogger(declaringType.FullName);
            logger.Error(exception,text);
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
            logger.Debug(exception,text);
        }

        static Log()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} | ${level} | ${message} | ${exception}"
            };
            config.AddTarget(consoleTarget);

            var fileTarget = new FileTarget("target2")
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout ="${longdate} | ${level} | Message: ${message} | Exception: ${exception}"
            };
            config.AddTarget(fileTarget);

            config.AddRuleForOneLevel(LogLevel.Debug, fileTarget);
            config.AddRuleForOneLevel(LogLevel.Error, fileTarget);
            config.AddRuleForAllLevels(consoleTarget);

            LogManager.Configuration = config;
            
        }

    }
}
