using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [AutoGenSO(GenPath = "Launch/Config")]
    public class LaunchConfigSO : ScriptableObject
    {
        public LAUNCH_ENVIRONMENT Environment;
        public LAUNCH_MODEL Model;
        public int Index;
    }
}
