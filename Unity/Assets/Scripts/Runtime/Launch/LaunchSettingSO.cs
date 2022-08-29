using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace TheMazeRunner
{
    public enum LaunchType
    {
        None,
        Combine,
        Client,
        Server,
    }
    [AutoGenSO(GenPath = "Launch")]
    public class LaunchSettingSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public string BuildOutputDir = "./Temp/Bin/Debug";
        [SerializeField]
        private LaunchType m_LaunchType = LaunchType.Combine;
#if UNITY_EDITOR
        public CodeOptimization CodeOptimization = CodeOptimization.Debug;
#endif
        private Dictionary<LaunchType, string> m_CodeDllNameDicts = new Dictionary<LaunchType, string>();
        private Dictionary<LaunchType, string[]> m_CodeDllPathDicts = new Dictionary<LaunchType, string[]>();

        private Dictionary<LaunchType, string[]> m_AwakeTypeDicts = new Dictionary<LaunchType, string[]>();

        public void OnAfterDeserialize()
        {
            //
            m_CodeDllNameDicts.Clear();
            m_CodeDllNameDicts.Add(LaunchType.Combine, "CombineCode");
            m_CodeDllNameDicts.Add(LaunchType.Client, "ClientCode");
            m_CodeDllNameDicts.Add(LaunchType.Server, "ServerCode");

            m_CodeDllPathDicts.Clear();
            m_CodeDllPathDicts.Add(LaunchType.Combine, new string[] { "Codes/Common/", "Codes/Client/", "Codes/Server/" });
            m_CodeDllPathDicts.Add(LaunchType.Client, new string[] { "Codes/Common/", "Codes/Client/" });
            m_CodeDllPathDicts.Add(LaunchType.Server, new string[] { "Codes/Common/", "Codes/Server/" });

            m_AwakeTypeDicts.Clear();
            m_AwakeTypeDicts.Add(LaunchType.Combine, new string[] { "ServerApp","ClientApp" });
            m_AwakeTypeDicts.Add(LaunchType.Client, new string[] { "ClientApp" });
            m_AwakeTypeDicts.Add(LaunchType.Server, new string[] { "ServerApp" });
        }
        public void OnBeforeSerialize()
        {

        }
        public string DllName
        {
            get
            {
                return m_CodeDllNameDicts[m_LaunchType];
            }
        }

        public string[] DllPath
        {
            get
            {
                return m_CodeDllPathDicts[m_LaunchType];
            }
        }

        public string[] AwakeType
        {
            get
            {
                return m_AwakeTypeDicts[m_LaunchType];
            }
        }
    }
}
