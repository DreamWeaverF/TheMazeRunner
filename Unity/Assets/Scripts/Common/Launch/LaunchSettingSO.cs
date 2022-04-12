using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [AutoGenSO(GenPath = "Launch/Setting")]
    public class LaunchSettingSO : ScriptableObject
    {
        public APP_ENVIRONMENT Environment;
        public APP_MODEL Model;
        public int Index;
    }
}
