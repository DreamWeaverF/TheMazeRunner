using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace TheMazeRunner
{
    public class Launch : MonoBehaviour
    {
        [SerializeField]
        private LaunchSettingSO m_LaunchSettingSO;

        private Action m_ActionUpdate;
        private Action m_ActionFixedUpdate;
        private Action m_ActionExit;

        void Awake()
        {
            UnityLog.Trace("Unity Awake");
#if UNITY_EDITOR
            byte[] assBytes = File.ReadAllBytes(Path.Combine(m_LaunchSettingSO.BuildOutputDir, $"{m_LaunchSettingSO.DllName}.dll"));
            byte[] pdbBytes = File.ReadAllBytes(Path.Combine(m_LaunchSettingSO.BuildOutputDir, $"{m_LaunchSettingSO.DllName}.pdb"));
#else
            //todolist load Addressable
#endif
            Assembly assembly = Assembly.Load(assBytes, pdbBytes);

            string spaceName = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
            UnityLog.Init(assembly, spaceName);

            Type type = assembly.GetType($"{spaceName}.AssembleHelper");
            MethodInfo methodInfo = type.GetMethod("Awake");
            methodInfo.Invoke(null, new object[0]);
            methodInfo = type.GetMethod("Update");
            m_ActionUpdate += methodInfo.CreateDelegate(typeof(Action)) as Action;
            methodInfo = type.GetMethod("FixedUpdate");
            m_ActionFixedUpdate += methodInfo.CreateDelegate(typeof(Action)) as Action;
            string[] awakeType = m_LaunchSettingSO.AwakeType;
            for (int i = 0; i < awakeType.Length; i++)
            {
                type = assembly.GetType($"{spaceName}.{awakeType[i]}");
                methodInfo = type.GetMethod("Awake");
                methodInfo.Invoke(Activator.CreateInstance(type), new object[0]);
            }
        }

        void Update()
        {
            m_ActionUpdate();
        }

        void FixedUpdate()
        {
            m_ActionFixedUpdate();
        }

        void OnDestroy()
        {
            //m_ActionExit();
        }
    }
}
