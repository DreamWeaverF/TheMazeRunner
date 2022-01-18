using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class PathHelper
    {
        public static string DevDllFilePath = "Library/ScriptAssemblies/{0}.dll";
        public static string DevPDBFilePath = "Library/ScriptAssemblies/{0}.pdb";

        public static string ReleaseDllFilePath = Application.streamingAssetsPath + "/ScriptAssemblies/{0}.dll";
        public static string ReleasePDBFilePath = Application.streamingAssetsPath + "/ScriptAssemblies/{0}.pdb";
    }
}

