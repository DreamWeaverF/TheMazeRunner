using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TheMazeRunner
{
    public class ScriptableObjectTool
    {
        private static readonly string soRootPath = "Assets/ScriptableObjects";
        private static readonly string[] dllNames = new string[] { "Client" , "Common" , "Server"};

        [MenuItem("Tools/AutoGenSO")]
        static void AutoGenSO()
        {
            for (int _i = 0; _i < dllNames.Length; _i++)
            {
                Assembly _asm = Assembly.LoadFile($"{Application.dataPath}/../Library/ScriptAssemblies/{dllNames[_i]}.dll");
                if (!AssetDatabase.IsValidFolder($"{soRootPath}/{dllNames[_i]}"))
                {
                    AssetDatabase.CreateFolder(soRootPath, dllNames[_i]);
                }
                Type[] _types = _asm.GetTypes();
                for (int _i1 = 0; _i1 < _types.Length; _i1++)
                {
                    AutoGenSOAttribute _att = _types[_i1].GetCustomAttribute<AutoGenSOAttribute>();
                    if (_att == null)
                    {
                        continue;
                    }

                    string[] _guids = AssetDatabase.FindAssets($"{_types[_i1].Name}", new[] { soRootPath });
                    if (_guids.Length > 0)
                    {
                        continue;
                    }
                    string _parentPath = $"{soRootPath}/{dllNames[_i]}/{_att.GenPath}";
                    string _savePath = $"{_parentPath}/{_types[_i1].Name}.asset";

                    if (!AssetDatabase.IsValidFolder(_parentPath))
                    {
                        string[] _pList = _att.GenPath.Split("/");
                        string _root = $"{soRootPath}/{dllNames[_i]}";
                        for (int _i2 = 0; _i2 < _pList.Length; _i2++)
                        {
                            if (!AssetDatabase.IsValidFolder(_root + "/" + _pList[_i2]))
                            {
                                AssetDatabase.CreateFolder(_root, _pList[_i2]);
                            }
                            _root += "/" + _pList[_i2];
                        }
                    }
                    Debug.Log($"AutoGen{_savePath}");
                    ScriptableObject _obj = ScriptableObject.CreateInstance(_types[_i1]);
                    AssetDatabase.CreateAsset(_obj, _savePath);
                }
                AssetDatabase.Refresh();
            }

        }
    }
}
