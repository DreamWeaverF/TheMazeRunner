using System;

namespace TheMazeRunner
{
    public static class LogHelper
    {
        //Trace
        public static Action<string> ActionLogTrace;
        public static void Trace(string msg)
        {
            ActionLogTrace(msg);
        }
        public static void Trace(string message, params object[] args)
        {
            Trace(string.Format(message, args));
        }
        //Debug
        public static Action<string> ActionLogDebug;
        public static void Debug(string msg)
        {
            ActionLogDebug(msg);
        }
        public static void Debug(string message, params object[] args)
        {
            Debug(string.Format(message, args));
        }
        //Info
        public static Action<string> ActionLogInfo;
        public static void Info(string msg)
        {
            ActionLogInfo(msg);
        }
        public static void Info(string message, params object[] args)
        {
            Info(string.Format(message, args));
        }
        //Warning
        public static Action<string> ActionLogWarning;
        public static void Warning(string msg)
        {
            ActionLogWarning(msg);
        }
        public static void Warning(string message, params object[] args)
        {
            Warning(string.Format(message, args));
        }
        //Error
        public static Action<string> ActionLogError;
        public static void Error(string msg)
        {
            ActionLogError(msg);
        }
        public static void Error(string message, params object[] args)
        {
            Error(string.Format(message, args));
        }
        //Fatal
        public static Action<string> ActionLogFatal;
        public static void Fatal(string msg)
        {
            ActionLogFatal(msg);
        }
        public static void Fatal(string message, params object[] args)
        {
            Fatal(string.Format(message, args));
        }
    }
}
