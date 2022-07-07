using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

namespace TheMazeRunner
{
    public static class BuildAssemblieEditor
    {
        //[MenuItem("Tools/Build/EnableAutoBuildCodeDebug _F1")]
        //public static void SetAutoBuildCode()
        //{
        //    PlayerPrefs.SetInt("AutoBuild", 1);
        //    ShowNotification("AutoBuildCode Enabled");
        //}
        
        //[MenuItem("Tools/Build/DisableAutoBuildCodeDebug _F2")]
        //public static void CancelAutoBuildCode()
        //{
        //    PlayerPrefs.DeleteKey("AutoBuild");
        //    ShowNotification("AutoBuildCode Disabled");
        //}
        private const string m_ConfigSOPath = "Assets/ScriptableObjects/Launch/LaunchSettingSO.asset";

        [MenuItem("Tools/Build/BuildCode _F5")]
        public static void BuildCode()
        {
            LaunchSettingSO launchConfigSO = AssetDatabase.LoadAssetAtPath<LaunchSettingSO>(m_ConfigSOPath);
            BuildMuteAssembly(launchConfigSO.DllName, launchConfigSO.DllPath, Array.Empty<string>(), launchConfigSO.CodeOptimization);
            //AfterCompiling();
            AssetDatabase.Refresh();
        }

        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences, CodeOptimization codeOptimization)
        {
            if (!Directory.Exists(Define.BuildOutputDir))
            {
                Directory.CreateDirectory(Define.BuildOutputDir);
            }
            List<string> scripts = new List<string>();
            for (int i = 0; i < CodeDirectorys.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(CodeDirectorys[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }

            string dllPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.pdb");
            File.Delete(dllPath);
            File.Delete(pdbPath);

            Directory.CreateDirectory(Define.BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());

            //启用UnSafe
            assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.additionalReferences = additionalReferences;
            
            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.buildStarted += delegate(string assemblyPath) { Debug.LogFormat("build start：" + assemblyPath); };

            assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat("有{0}个Warning!!!", warningCount);
                }

                if (errorCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error)
                        {
                            Debug.LogError(compilerMessages[i].message);
                        }
                    }
                }
            };
            
            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
                return;
            }
        }

        private static void AfterCompiling()
        {
            while (EditorApplication.isCompiling)
            {
                Debug.Log("Compiling wait1");
                // 主线程sleep并不影响编译线程
                Thread.Sleep(1000);
                Debug.Log("Compiling wait2");
            }
            
            Debug.Log("Compiling finish");

            //Directory.CreateDirectory(CodeDir);
            //File.Copy(Path.Combine(Define.BuildOutputDir, "ClientCode.dll"), Path.Combine(CodeDir, "ClientCode.dll.bytes"), true);
            //File.Copy(Path.Combine(Define.BuildOutputDir, "ClientCode.pdb"), Path.Combine(CodeDir, "ClientCode.pdb.bytes"), true);
            //AssetDatabase.Refresh();
            //Debug.Log("copy Code.dll to Bundles/Code success!");
            
            //// 设置ab包
            //AssetImporter assetImporter1 = AssetImporter.GetAtPath("Assets/Bundles/Code/ClientCode.dll.bytes");
            //assetImporter1.assetBundleName = "ClientCode.unity3d";
            //AssetImporter assetImporter2 = AssetImporter.GetAtPath("Assets/Bundles/Code/ClientCode.pdb.bytes");
            //assetImporter2.assetBundleName = "ClientCode.unity3d";
            //AssetDatabase.Refresh();
            //Debug.Log("set assetbundle success!");
            //Debug.Log("build success!");
            ////反射获取当前Game视图，提示编译完成
            //ShowNotification("Build Code Success");
        }

        public static void ShowNotification(string tips)
        {
            var game = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
            game?.ShowNotification(new GUIContent($"{tips}"));
        }
    }
    
}