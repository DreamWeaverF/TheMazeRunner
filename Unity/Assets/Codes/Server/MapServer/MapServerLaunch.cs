using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class MapServerLaunch
    {
        public static void Start(LAUNCH_ENVIRONMENT _environment)
        {
            LogHelper.Log($"Start MapServer ##Environment: {_environment}");
        }
    }
}
