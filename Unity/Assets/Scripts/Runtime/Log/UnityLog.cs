using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TheMazeRunner
{
    public static class UnityLog
    {
        public static void Init(Assembly assembly)
        {
            Type logHelperType = assembly.GetType("TheMazeRunner.LogHelper");

            FieldInfo actionField = logHelperType.GetField("ActionLogTrace");
            Action<string> action = actionField.GetValue(null) as Action<string>;
            action += Trace;
            actionField.SetValue(null, action);

            actionField = logHelperType.GetField("ActionLogDebug");
            action = actionField.GetValue(null) as Action<string>;
            action += Debug;
            actionField.SetValue(null, action);

            actionField = logHelperType.GetField("ActionLogInfo");
            action = actionField.GetValue(null) as Action<string>;
            action += Info;
            actionField.SetValue(null, action);

            actionField = logHelperType.GetField("ActionLogWarning");
            action = actionField.GetValue(null) as Action<string>;
            action += Warning;
            actionField.SetValue(null, action);

            actionField = logHelperType.GetField("ActionLogError");
            action = actionField.GetValue(null) as Action<string>;
            action += Error;
            actionField.SetValue(null, action);

            actionField = logHelperType.GetField("ActionLogFatal");
            action = actionField.GetValue(null) as Action<string>;
            action += Fatal;
            actionField.SetValue(null, action);
        }
        internal static void Trace(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        internal static void Debug(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        internal static void Info(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        internal static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }
        internal static void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
        internal static void Fatal(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
