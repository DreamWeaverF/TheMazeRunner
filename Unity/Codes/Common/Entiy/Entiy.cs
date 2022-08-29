using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheMazeRunner
{
    public class Entiy : IDisposable
    {
        private Action m_UpdateAction;
        private Action m_FixedUpdateAction;
        public Entiy()
        {
            MethodInfo[] methods = this.GetType().GetMethods();
            MethodInfo tempMethod;
            for (int i = 0; i < methods.Length; i++)
            {
                tempMethod = methods[i];
                var atts = tempMethod.GetCustomAttributes<BaseAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is UpdateMethodAttribute)
                    {
                        m_UpdateAction = tempMethod.CreateAction(this);
                        m_UpdateAction.RegisterUpdateMethod();
                    }
                    if (att is FixedUpdateMethodAttribute)
                    {
                        m_FixedUpdateAction = tempMethod.CreateAction(this);
                        m_FixedUpdateAction.RegisterFixedUpdateMethod();
                    }
                    if(att is SynchronizeMethodAttribute)
                    {

                    }
                }
            }
        }
        public void Dispose()
        {
            LogHelper.Debug("Dispose");
            if (m_UpdateAction != null)
            {
                m_UpdateAction.UnRegisterUpdateMethod();
                m_UpdateAction = null;
            }
            if (m_FixedUpdateAction != null)
            {
                m_FixedUpdateAction.UnRegisterFixedUpdateMethod();
                m_FixedUpdateAction = null;
            }
        }
    }
}
