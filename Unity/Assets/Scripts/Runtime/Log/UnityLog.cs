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
            FieldInfo logActionField = assembly.GetType("TheMazeRunner.LogHelper").GetField("ActionLogTrace");
            Action<string> logAction = logActionField.GetValue(null) as Action<string>;
            logAction += Trace;
        }
        public static void Trace(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        public static void Debug(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        public static void Info(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        public static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }
        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
        public static void Fatal(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
