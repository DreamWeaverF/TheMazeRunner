using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class ClientLaunch
    {
        public static void Start(LAUNCH_ENVIRONMENT _environment)
        {
            LogHelper.Log($"Start Client ##Environment: {_environment}");
        }
    }
}


