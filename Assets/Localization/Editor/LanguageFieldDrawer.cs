using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Localization
{
    [CustomPropertyDrawer(typeof(LanguageField<>), true)]
    public class LanguageFieldDrawer: PropertyDrawer
    {
        private LocalizationSettings settings;
        private bool foldout;

        private void LoadSettings()
        {
            if (settings == null)
                settings = SettingsLoader.LoadSettings();
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LoadSettings();
            EditorGUI.BeginProperty(position, label, property);
            foldout = EditorGUI.Foldout(position, foldout, property.name);
            if (!foldout) return;
            EditorGUI.indentLevel = 1;
            var langArr = LangMethods.LangArr(settings);
            
            for (var i = 0; i < langArr.Length; i++)
            {
                var enableLang = langArr[i];
                if (!enableLang) return;
                
                EditorGUILayout.PropertyField(
                    property.FindPropertyRelative(LangMethods.LangName(i)),
                    new GUIContent(LangMethods.LangName(i))
                );
            }
            EditorGUI.indentLevel = 0;
            EditorGUI.EndProperty();
        }
        
    }
}