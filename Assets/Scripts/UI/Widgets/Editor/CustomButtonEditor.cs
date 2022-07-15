using System.Collections;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Assets.Scripts.UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton))]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}