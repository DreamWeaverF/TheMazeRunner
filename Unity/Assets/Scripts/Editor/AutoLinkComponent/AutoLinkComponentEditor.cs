using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TheMazeRunner
{
    [CustomEditor(typeof(AutoLinkComponentMono), true)]
    public class AutoLinkComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AutoLinkComponentMono component = (AutoLinkComponentMono)target;
            if (GUILayout.Button("LinkData"))
            {
                serializedObject.Update();
                FieldInfo[] props = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                if (props != null && props.Length >= 0)
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        FieldInfo prop = props[i];
                        if (prop.GetCustomAttribute<SerializeField>() == null)
                        {
                            continue;
                        }
                        SerializedProperty sProperty = serializedObject.FindProperty(prop.Name);
                        Transform temp = component.transform.FindTargetChild(prop.Name);
                        if (temp != null)
                        {
                            sProperty.objectReferenceValue = temp.GetComponent(prop.FieldType.Name);
                            Debug.Log($"关联成功{prop.Name}");
                        }
                    }
                }
                EditorUtility.SetDirty(this);
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfRequiredOrScript();
            }
        }
    }
}
