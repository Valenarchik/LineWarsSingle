using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Localization
{
    [CustomPropertyDrawer(typeof(LanguageField<>), true)]
    [CustomPropertyDrawer(typeof(LanguageTextAreaAttribute), true)]
    public class LanguageFieldDrawer: PropertyDrawer
    {
        private LocalizationSettings settings;

        private void LoadSettings()
        {
            if (settings == null)
                settings = SettingsLoader.LoadSettings();
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LoadSettings();
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName);
            if (!property.isExpanded) return;
            EditorGUI.indentLevel += 1;
            var langArr = LangMethods.LangArr(settings);
            
            for (var i = 0; i < langArr.Length; i++)
            {
                var enableLang = langArr[i];
                if (!enableLang) continue;

                var relativeProperty = property.FindPropertyRelative(LangMethods.LangName(i));
                
                if (attribute is LanguageTextAreaAttribute textArea 
                    && relativeProperty.propertyType == SerializedPropertyType.String)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(LangMethods.LangName(i), GUILayout.Width(35), GUILayout.Height(20));
                    
                    relativeProperty.stringValue = EditorGUILayout.TextArea(
                        relativeProperty.stringValue,
                        EditorStyles.textArea,
                        GUILayout.Height(textArea.Height));
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(
                       relativeProperty,
                        new GUIContent(LangMethods.LangName(i))
                    );
                }
            }
            EditorGUILayout.LabelField("", GUILayout.Height(5));
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }
    }
}