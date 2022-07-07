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
        //public const 

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
            UnityLog.Init(assembly);

            Type[] types = assembly.GetTypes();
            MethodInfo methodInfo;
            Action action;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].GetInterface("IUpdate") != null)
                {
                    methodInfo = types[i].GetMethod("Update");
                    action = methodInfo.CreateDelegate(typeof(Action), null) as Action;
                    m_ActionUpdate += action;
                }
                if (types[i].GetInterface("IFixedUpdate") != null)
                {
                    methodInfo = types[i].GetMethod("FixedUpdate");
                    action = methodInfo.CreateDelegate(typeof(Action), null) as Action;
                    m_ActionFixedUpdate += action;
                }
            }
            string[] awakeType = m_LaunchSettingSO.AwakeType;
            for (int i = 0; i < awakeType.Length; i++)
            {
                methodInfo = assembly.GetType(awakeType[i]).GetMethod("Awake");
                methodInfo.Invoke(null, new object[0]);
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
