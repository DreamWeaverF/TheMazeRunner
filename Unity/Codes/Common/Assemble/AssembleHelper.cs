using System;
using System.Reflection;

namespace TheMazeRunner
{
    public static class AssembleHelper
    {
        private static Action m_ActionUpdate;
        private static Action m_ActionFixedUpdate;

        public static void Awake()
        {
            Assembly assembly = typeof(AssembleHelper).Assembly;
            Type[] types = assembly.GetTypes();
            Type type;
            FieldInfo[] filedInfos;
            FieldInfo fi;
            for (int i = 0; i < types.Length; i++)
            {
                type = types[i];
                filedInfos = type.GetFields();
                for(int i1 = 0; i1 < filedInfos.Length; i1++)
                {
                    fi = filedInfos[i1];
                    BroadcastFieldAttribute att = fi.GetCustomAttribute<BroadcastFieldAttribute>();
                    if(att != null)
                    {

                    }
                }
            }
        }

        public static void Update()
        {
            if(m_ActionUpdate != null)
            {
                m_ActionUpdate();
            }
        }

        public static void FixedUpdate()
        {
            if (m_ActionFixedUpdate != null)
            {
                m_ActionFixedUpdate();
            }
        }
        public static Action CreateAction(this MethodInfo methodInfo, object target)
        {
            return methodInfo.CreateDelegate(typeof(Action), target) as Action;
        }

        internal static void RegisterUpdateMethod(this Action action)
        {
            LogHelper.Debug("RegisterUpdateMethod");
            m_ActionUpdate += action;
        }

        internal static void UnRegisterUpdateMethod(this Action action)
        {
            LogHelper.Debug("UnRegisterUpdateMethod");
            m_ActionUpdate -= action;
        }

        internal static void RegisterFixedUpdateMethod(this Action action)
        {
            m_ActionFixedUpdate += action;
        }

        internal static void UnRegisterFixedUpdateMethod(this Action action)
        {
            m_ActionFixedUpdate -= action;
        }

        public static T CreateInstance<T>() where T : class, IDisposable
        {
            using T t = Activator.CreateInstance<T>();
            return t;
        }
    }
}
