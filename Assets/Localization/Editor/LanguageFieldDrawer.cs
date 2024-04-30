using UnityEditor;
using UnityEngine;

namespace Localization
{
    [CustomPropertyDrawer(typeof(LanguageField<>), true)]
    [CustomPropertyDrawer(typeof(LanguageTextAreaAttribute), true)]
    public class LanguageFieldDrawer: PropertyDrawer
    {
        private LocalizationSettings settings;
        private GUIStyle red;
        private GUIStyle green;

        private void LoadSettings()
        {
            if (settings == null)
                settings = SettingsLoader.LoadSettings();
        }

        private void InitializeColors()
        {
            red = new GUIStyle(EditorStyles.label)
            {
                normal =
                {
                    textColor = Color.red
                }
            };

            green = new GUIStyle(EditorStyles.label)
            {
                normal =
                {
                    textColor = Color.green
                }
            };
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }
            
            LoadSettings();
            InitializeColors();
            
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName);
            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }
            
            EditorGUI.indentLevel += 1;
            
            DrawAutoLocalizationIfAvailable(position, property, label);
            DrawManualIfAvailable(position, property, label);
            DrawCsvIfAvailable(position, property, label);
            DrawFields(position, property, label);
            
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        private void DrawAutoLocalizationIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.AutoLocalization)
                return;
            EditorGUILayout.LabelField("> Auto Localization", green);
        }
        private void DrawManualIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.Manual)
                return;
            EditorGUILayout.LabelField("> Manual Localization", green);
        }

        private void DrawCsvIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.CSVFile) return;
            EditorGUILayout.LabelField("> CSV Localization (support only string type)", green);
            
            
            if (property.FindPropertyRelative("ru").propertyType != SerializedPropertyType.String)
                return;
            
            var identity = property.FindPropertyRelative("identity");

            GUILayout.BeginVertical("HelpBox");
                
            GUILayout.BeginHorizontal();
                
            var availableStr = !string.IsNullOrEmpty(identity.stringValue);
            identity.stringValue = EditorGUILayout.TextField(identity.stringValue, GUILayout.Height(20));
                
            if (availableStr)
            {
                GUILayout.Label("ID Translate");
            }
            else
            {
                GUILayout.Label("ID Translate (necessarily)", red);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Import"))
            {
                var transfers = CSVManager.ImportTransfersByKey(settings, identity.stringValue);
                if (transfers != null)
                    SetLanguages(property, transfers);
            }

            if (GUILayout.Button("Export"))
            {
                CSVManager.SetIDLineFile(settings, identity.stringValue, GetLanguages(property));
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void SetLanguages(SerializedProperty property, string[] values)
        {
            for (var i = 0; i < settings.languages.Count; i++)
            {
                var langProperty = property.FindPropertyRelative(LangMethods.LangName(i));
                langProperty.stringValue = values[i];
            }
        }
        
        private string[] GetLanguages(SerializedProperty property)
        {
            var result = new string[settings.languages.Count];
            for (var i = 0; i < settings.languages.Count; i++)
            {
                var langProperty = property.FindPropertyRelative(LangMethods.LangName(i));
                result[i] = langProperty.stringValue;
            }

            return result;
        }
        
        private void DrawFields(Rect position, SerializedProperty property, GUIContent label)
        {
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
        }
    }
}