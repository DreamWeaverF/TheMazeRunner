using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoGenSOAttribute : PropertyAttribute
    {
        public string GenPath;
    }
}
