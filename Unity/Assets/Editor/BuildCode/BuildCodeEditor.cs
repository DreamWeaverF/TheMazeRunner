using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.AddressableAssets;

namespace TheMazeRunner
{
    public static class BuildCodeEditor
    {
        private const string CodeDir = "Assets/Code/";
        public const string BuildOutputDir = "./Temp/Bin/Debug";

        [MenuItem("Tools/BuildCodeDebug _F5")]
        public static void BuildCodeDebug()
        {
            BuildMuteAssembly("Client", new[]
            {
                "../Code/Client/",
            }, Array.Empty<string>(), CodeOptimization.Debug);

            AfterCompiling();

            AssetDatabase.Refresh();
        }
        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences, CodeOptimization codeOptimization)
        {
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

            string dllPath = Path.Combine(BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(BuildOutputDir, $"{assemblyName}.pdb");
            File.Delete(dllPath);
            File.Delete(pdbPath);

            Directory.CreateDirectory(BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());

            //����UnSafe
            //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.additionalReferences = additionalReferences;

            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 ��������
            //AssemblyBuilderFlags.DevelopmentBuild     ����ģʽ���
            //AssemblyBuilderFlags.EditorAssembly       �༭��״̬
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.buildStarted += delegate (string assemblyPath) { Debug.LogFormat("build start��" + assemblyPath); };

            assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat("��{0}��Warning!!!", warningCount);
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

            //��ʼ����
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail��" + assemblyBuilder.assemblyPath);
                return;
            }
        }

        private static void AfterCompiling()
        {
            while (EditorApplication.isCompiling)
            {
                Debug.Log("Compiling wait1");
                // ���߳�sleep����Ӱ������߳�
                Thread.Sleep(1000);
                Debug.Log("Compiling wait2");
            }

            Debug.Log("Compiling finish");

            Directory.CreateDirectory(CodeDir);
            File.Copy(Path.Combine(BuildOutputDir, "Client.dll"), Path.Combine(CodeDir, "Client.dll.bytes"), true);
            File.Copy(Path.Combine(BuildOutputDir, "Client.pdb"), Path.Combine(CodeDir, "Client.pdb.bytes"), true);

            AssetDatabase.Refresh();
            Debug.Log("copy Code.dll to Bundles/Code success!");

            UnityEngine.Object dllObject = AssetDatabase.LoadAssetAtPath(Path.Combine(CodeDir, "Client.dll.bytes"), typeof(TextAsset));
            AddressablesEditor.ModifyAddressableAssetEntry(dllObject, "Code", "Client.dll");

            UnityEngine.Object pdbObject = AssetDatabase.LoadAssetAtPath(Path.Combine(CodeDir, "Client.pdb.bytes"), typeof(TextAsset));
            AddressablesEditor.ModifyAddressableAssetEntry(pdbObject, "Code", "Client.pdb");

            AssetDatabase.Refresh();
            Debug.Log("set assetbundle success!");

            Debug.Log("build success!");
        }
    }
}

