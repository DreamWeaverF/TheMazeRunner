using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class LogHelper 
    {
        public static void Log(string _message)
        {
            Debug.Log(_message);
        }

        public static void LogWarning(string _message)
        {
            Debug.LogWarning(_message);
        }

        public static void LogError(string _message)
        {
            Debug.LogError(_message);
        }
    }
}
