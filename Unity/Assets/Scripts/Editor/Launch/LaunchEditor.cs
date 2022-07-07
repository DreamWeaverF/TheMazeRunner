using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheMazeRunner
{
    [CustomEditor(typeof(Launch))]
    public class LaunchEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty sp = serializedObject.FindProperty("m_LaunchSettingSO");
            EditorGUILayout.PropertyField(sp, true);
            if (sp.objectReferenceValue != null)
            {
                CreateEditor((LaunchSettingSO)sp.objectReferenceValue).OnInspectorGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
