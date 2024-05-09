using UnityEditor;
using UnityEngine;

namespace Localization
{
    [CustomPropertyDrawer(typeof(LanguageField<>), true)]
    [CustomPropertyDrawer(typeof(LanguageTextAreaAttribute), true)]
    [CustomPropertyDrawer(typeof(LanguageFieldAttribute), true)]
    public class LanguageFieldDrawer : PropertyDrawer
    {
        private LocalizationSettings settings;
        private GUIStyle red;
        private GUIStyle green;
        private GUIStyle button;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                return base.GetPropertyHeight(property, label);
            }

            LoadSettings();
            var height = EditorGUIUtility.singleLineHeight;
            if (!property.isExpanded)
                return height;

            height += GetManualIfAvailableHeight(property, label);
            height += GetAutoLocalizationIfAvailableHeight(property, label);
            height += GetCsvIfAvailableHeight(property, label);
            height += GetFieldsHeight(property);
            return height + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            LoadSettings();
            InitializeStyles();

            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(position.SetHeight(EditorGUIUtility.singleLineHeight), property.isExpanded,
                property.displayName);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel += 1;
            position = position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            position = DrawAutoLocalizationIfAvailable(position, property, label);
            position = DrawManualIfAvailable(position, property, label);
            position = DrawCsvIfAvailable(position, property, label);
            DrawFields(position, property, label);
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        private void DrawFields(Rect position, SerializedProperty property, GUIContent label)
        {
            var textHeight = property.FindPropertyRelative("textHeight");
            var isString = IsString(property);
            
            EditorGUI.DrawRect(position
                .SetHeight(GetFieldsHeight(property)),
                new Color(0.2f,0.2f,0.2f));
            
            var activeLanguages = LangMethods.GetActiveLanguages(settings);
            EditorGUI.LabelField(position
                .SetHeight(EditorGUIUtility.singleLineHeight),
                $"{activeLanguages.Length} Languages");
            position = position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            if (isString)
            {
                var leftValue = 20f;
                var rightValue = 500f;
                if (attribute is LanguageTextAreaAttribute textAreaAttribute)
                {
                    leftValue = textAreaAttribute.MinHeight;
                    rightValue = textAreaAttribute.MaxHeight;
                }
                
                textHeight.floatValue = EditorGUI.Slider(position.SetHeight(EditorGUIUtility.singleLineHeight), "Text height", textHeight.floatValue, leftValue, rightValue);
                position = position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }
            
            foreach (var lang in activeLanguages)
            {
                var langString = LangMethods.GetLangName(lang);
                var relativeProperty = property.FindPropertyRelative(langString);

                if (isString)
                {
                    EditorGUI.LabelField(position.SetWidth(40).SetHeight(textHeight.floatValue), langString);
                    relativeProperty.stringValue = EditorGUI.TextArea(
                        position.SetHeight(textHeight.floatValue)
                            .SetX(40)
                            .AddWidth(-30-EditorGUIUtility.standardVerticalSpacing),
                        relativeProperty.stringValue,
                        EditorStyles.textArea);
                    position = position.AddY(textHeight.floatValue + EditorGUIUtility.standardVerticalSpacing);
                }
                else
                {
                    var relativePropertyHeight = EditorGUI.GetPropertyHeight(relativeProperty);
                    EditorGUI.PropertyField(
                        position.SetHeight(relativePropertyHeight),
                        relativeProperty,
                        new GUIContent(langString)
                    );
                    position = position.AddY(relativePropertyHeight + EditorGUIUtility.standardVerticalSpacing);
                }
            }
        }

        private float GetFieldsHeight(SerializedProperty property)
        {
            var textHeight = property.FindPropertyRelative("textHeight");
            var isString = IsString(property);
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (isString)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            foreach (var lang in LangMethods.GetActiveLanguages(settings))
            {
                var langString = LangMethods.GetLangName(lang);
                var relativeProperty = property.FindPropertyRelative(langString);

                if (isString)
                {
                    height += textHeight.floatValue + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    height += EditorGUI.GetPropertyHeight(relativeProperty) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }

        private float GetAutoLocalizationIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.AutoLocalization)
                return 0;
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private Rect DrawAutoLocalizationIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.AutoLocalization)
                return position;
            EditorGUI.LabelField(position.SetHeight(EditorGUIUtility.singleLineHeight), "> Auto Localization (not support now)", green);
            return position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }

        private float GetManualIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.Manual)
                return 0;
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private Rect DrawManualIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.Manual)
                return position;
            EditorGUI.LabelField(position.SetHeight(EditorGUIUtility.singleLineHeight), "> Manual Localization", green);
            return position.AddY(EditorGUIUtility.singleLineHeight);
        }
        
        private float GetCsvIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.CSVFile)
                return 0;
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (property.FindPropertyRelative("ru").propertyType != SerializedPropertyType.String)
                return height;
            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
            return height;
        }

        private Rect DrawCsvIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.CSVFile)
                return position;
            EditorGUI.LabelField(
                position.SetHeight(EditorGUIUtility.singleLineHeight),
                "> CSV Localization (support only string type)",
                green);
            position = position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            if (!IsString(property))
                return position;

            var identity = property.FindPropertyRelative("identity");
            identity.stringValue = EditorGUI.TextField(
                position
                    .WidthPercent(0.65f)
                    .SetHeight(EditorGUIUtility.singleLineHeight),
                identity.stringValue);

            var helperPosition = position
                .RelativeX(0.65f)
                .AddX(EditorGUIUtility.standardVerticalSpacing)
                .WidthPercent(0.4f)
                .SetHeight(EditorGUIUtility.singleLineHeight);

            if (!string.IsNullOrEmpty(identity.stringValue))
                EditorGUI.LabelField(helperPosition, "ID Translate");
            else
                EditorGUI.LabelField(helperPosition, "ID Translate (necessarily)", red);
            
            position = position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            if (GUI.Button(position
                    .AddX(EditorGUI.indentLevel * 15)
                    .SubtractWidth(EditorGUI.indentLevel * 15 + EditorGUIUtility.standardVerticalSpacing)
                    .WidthPercent(0.5f)
                    .SetHeight(EditorGUIUtility.singleLineHeight), "Import", button))
            {
                var transfers = CSVManager.ImportTransfersByKey(settings, identity.stringValue);
                if (transfers != null)
                    SetLanguages(property, transfers);
            }

            if (GUI.Button(position
                    .RelativeX(0.5f)
                    .AddX(EditorGUI.indentLevel * 15)
                    .SubtractWidth(EditorGUI.indentLevel * 15)
                    .WidthPercent(0.5f)
                    .SubtractWidth(EditorGUIUtility.standardVerticalSpacing * 4)
                    .SetHeight(EditorGUIUtility.singleLineHeight), "Export", button))
            {
                CSVManager.SetIDLineFile(settings, identity.stringValue, GetLanguages(property));
            }

            return position.AddY(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }

        private static bool IsString(SerializedProperty property)
        {
            return property.FindPropertyRelative("ru").propertyType == SerializedPropertyType.String;
        }

        private void SetLanguages(SerializedProperty property, string[] values)
        {
            for (var i = 0; i < settings.languages.Count; i++)
            {
                var langProperty = property.FindPropertyRelative(LangMethods.GetLangName(i));
                langProperty.stringValue = values[i];
            }
        }

        private string[] GetLanguages(SerializedProperty property)
        {
            var result = new string[settings.languages.Count];
            for (var i = 0; i < settings.languages.Count; i++)
            {
                var langProperty = property.FindPropertyRelative(LangMethods.GetLangName(i));
                result[i] = langProperty.stringValue;
            }

            return result;
        }

        private void LoadSettings()
        {
            if (settings == null)
                settings = SettingsLoader.LoadSettings();
        }

        private void InitializeStyles()
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
            
            button = new GUIStyle(EditorStyles.miniButton);
        }
    }
}