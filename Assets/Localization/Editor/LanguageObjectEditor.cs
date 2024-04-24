using System;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    [CustomEditor(typeof(LanguageObject<>), true)]
    public class LanguageObjectEditor: Editor
    {
        private LocalizationSettings settings;
        private bool foldout;

        private void OnEnable()
        {
            settings = SettingsLoader.LoadSettings();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UpdateLanguages();
        }
        
        void UpdateLanguages()
        {
            foldout = EditorGUILayout.Foldout(foldout, new GUIContent("Languages"));
            if (!foldout) return;
            EditorGUI.indentLevel = 1;
            serializedObject.Update();
            var langArr = LangMethods.LangArr(settings);

            for (var i = 0; i < langArr.Length; i++)
            {
                if (!langArr[i]) continue;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(LangMethods.LangName(i)),
                    new GUIContent(LangMethods.LangName(i)));
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = 0;
        }
    }
}