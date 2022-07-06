
using System;
using System.Reflection;
using UnityEngine;

namespace TheMazeRunner
{
    public class AutoLinkCompoent
    {
        private GameObject m_TargetObject;
        public virtual void Awake()
        {
            GameObject targetPrefab = Resources.Load<GameObject>(this.GetType().Name);
            if (targetPrefab == null)
            {
                return;
            }
            m_TargetObject = GameObject.Instantiate(targetPrefab);
            m_TargetObject.SetActive(true);

            AutoLinkComponentMono linkCom = m_TargetObject.GetComponent<AutoLinkComponentMono>();
            if (linkCom == null)
            {
                return;
            }
            FieldInfo[] linkFileds = linkCom.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo[] useFileds = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < useFileds.Length; i++)
            {
                for (int j = 0; j < linkFileds.Length; j++)
                {
                    if (useFileds[i].Name == linkFileds[j].Name)
                    {
                        useFileds[i].SetValue(this, linkFileds[j].GetValue(linkCom));
                    }
                }
            }
        }

        public virtual void Destory()
        {
            GameObject.Destroy(m_TargetObject);
            m_TargetObject = null;
        }
    }

    public class AutoLinkFieldAttribute : Attribute
    {

    }
}

