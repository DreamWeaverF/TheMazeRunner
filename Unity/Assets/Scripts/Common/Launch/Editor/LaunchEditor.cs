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
            var settingDataSO = serializedObject.FindProperty("settingSO");
            EditorGUILayout.PropertyField(settingDataSO, true);
            if (settingDataSO.objectReferenceValue != null)
            {
                CreateEditor((LaunchSettingSO)settingDataSO.objectReferenceValue).OnInspectorGUI();
            }
        }
    }
}
