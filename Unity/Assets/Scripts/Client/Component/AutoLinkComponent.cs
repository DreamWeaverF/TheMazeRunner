using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TheMazeRunner
{
    [CustomEditor(typeof(AutoLinkComponent), true)]
    public class AutoLinkComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AutoLinkComponent component = (AutoLinkComponent)target;
            if (GUILayout.Button("LinkData"))
            {
                component.GenLinkData();
            }
        }
    }

    public class AutoLinkComponent : MonoBehaviour
    {
        private List<object> mSerializeDatas = new List<object>();
        [SerializeField]
        private byte[] mBytes;
        public void GenLinkData()
        {
            mSerializeDatas.Clear();
            FieldInfo[] props = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<SerializeField>() == null)
                {
                    continue;
                }
                Transform temp = this.transform.FindTargetChild(prop.Name);
                if (temp != null)
                {
                    prop.SetValue(this, temp.GetComponent(prop.FieldType.Name));
                    Debug.Log($"关联成功{prop.Name}");
                }
                mSerializeDatas.Add(prop.GetValue(this));
            }
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, mSerializeDatas);
            mBytes = memStream.ToArray();
            Debug.Log("生成结束");
        }


    }
}
