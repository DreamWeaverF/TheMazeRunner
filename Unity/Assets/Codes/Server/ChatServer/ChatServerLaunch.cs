using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class ChatServerLaunch
    {
        public static void Start(LAUNCH_ENVIRONMENT _environment)
        {
            LogHelper.Log($"Start ChatServer ##Environment: {_environment}");
        }
    }
}
