using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class CenterServerLaunch
    {
        // Start is called before the first frame update
        public static void Start(LAUNCH_ENVIRONMENT _environment)
        {
            LogHelper.Log($"Start CenterServer ##Environment: {_environment}");
        }
    }
}
