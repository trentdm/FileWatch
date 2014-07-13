using System;
using NLog;

namespace FileWatch.Utils
{
    public static class Log
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void LogTrace(this object source, string message, params object[] args)
        {
            Logger.Trace(message, args);
        }

        public static void LogDebug(this object source, string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        public static void LogInfo(this object source, string message, params object[] args)
        {
            Logger.Info(message, args);
        }

        public static void LogError(this object source, Exception exception)
        {
            Logger.Error(exception);
        }

        public static void LogFatal(this object source, Exception exception)
        {
            Logger.Fatal(exception);
        }
    }
}
