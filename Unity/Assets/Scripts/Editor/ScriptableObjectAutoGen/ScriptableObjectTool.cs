using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TheMazeRunner
{
    public class ScriptableObjectTool
    {
        private static readonly string m_SoRootPath = "Assets/ScriptableObjects";
        private static readonly string[] m_DllNames = new string[] { "Runtime" };
        private static readonly string m_CacheDllPath = "{0}/../Library/ScriptAssemblies/{1}.dll";

        [MenuItem("Tools/AutoGenSO")]
        static void AutoGenSO()
        {
            for (int i = 0; i < m_DllNames.Length; i++)
            {
                Assembly asm = Assembly.LoadFile(string.Format(m_CacheDllPath, Application.dataPath, m_DllNames[i]));
                Type[] types = asm.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    AutoGenSOAttribute att = types[j].GetCustomAttribute<AutoGenSOAttribute>();
                    if (att == null)
                    {
                        continue;
                    }

                    string[] guids = AssetDatabase.FindAssets($"{types[j].Name}", new[] { m_SoRootPath });
                    if (guids.Length > 0)
                    {
                        continue;
                    }
                    string parentPath = $"{m_SoRootPath}/{att.GenPath}";
                    string savePath = $"{parentPath}/{types[j].Name}.asset";

                    if (!AssetDatabase.IsValidFolder(parentPath))
                    {
                        string[] pList = att.GenPath.Split("/");
                        string root = m_SoRootPath;
                        for (int k = 0; k < pList.Length; k++)
                        {
                            if (!AssetDatabase.IsValidFolder(root + "/" + pList[k]))
                            {
                                AssetDatabase.CreateFolder(root, pList[k]);
                            }
                            root += "/" + pList[k];
                        }
                    }
                    Debug.Log($"AutoGen{savePath}");
                    ScriptableObject so = ScriptableObject.CreateInstance(types[j]);
                    AssetDatabase.CreateAsset(so, savePath);
                }
                AssetDatabase.Refresh();
            }
        }
    }
}
