using UnityEditor;
using UnityEngine;

namespace Localization
{
    [CustomPropertyDrawer(typeof(LanguageField<>), true)]
    [CustomPropertyDrawer(typeof(LanguageTextAreaAttribute), true)]
    [CustomPropertyDrawer(typeof(LanguageFieldAttribute), true)]
    public class LanguageFieldDrawer : PropertyDrawer
    {
        private const float FOLDOUT_HEIGHT = 16f;
        private const float SPACE = 2f;
        private const float LABEL_HEIGHT = 20;

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
            var height = FOLDOUT_HEIGHT;
            if (!property.isExpanded)
                return height;

            height += GetManualIfAvailableHeight(property, label);
            height += GetAutoLocalizationIfAvailableHeight(property, label);
            height += GetCsvIfAvailableHeight(property, label);

            foreach (var lang in LangMethods.GetActiveLanguages(settings))
            {
                var langString = LangMethods.GetLangName(lang);
                var relativeProperty = property.FindPropertyRelative(langString);

                if (attribute is LanguageTextAreaAttribute textArea
                    && relativeProperty.propertyType == SerializedPropertyType.String)
                {
                    height += textArea.Height + SPACE;
                }
                else
                {
                    height += EditorGUI.GetPropertyHeight(relativeProperty) + SPACE;
                }
            }

            return height + SPACE;
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
            property.isExpanded = EditorGUI.Foldout(position.SetHeight(FOLDOUT_HEIGHT), property.isExpanded,
                property.displayName);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel += 1;
            position = position.AddY(FOLDOUT_HEIGHT + SPACE);
            position = DrawAutoLocalizationIfAvailable(position, property, label);
            position = DrawManualIfAvailable(position, property, label);
            position = DrawCsvIfAvailable(position, property, label);
            DrawFields(position, property, label);
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        private void DrawFields(Rect position, SerializedProperty property, GUIContent label)
        {
            foreach (var lang in LangMethods.GetActiveLanguages(settings))
            {
                var langString = LangMethods.GetLangName(lang);
                var relativeProperty = property.FindPropertyRelative(langString);

                if (attribute is LanguageTextAreaAttribute textArea
                    && relativeProperty.propertyType == SerializedPropertyType.String)
                {
                    EditorGUI.LabelField(position.SetWidth(40).SetHeight(textArea.Height), langString);
                    relativeProperty.stringValue = EditorGUI.TextArea(
                        position.SetHeight(textArea.Height).SetX(40).AddWidth(-20-SPACE),
                        relativeProperty.stringValue,
                        EditorStyles.textArea);
                    position = position.AddY(textArea.Height + SPACE);
                }
                else
                {
                    var relativePropertyHeight = EditorGUI.GetPropertyHeight(relativeProperty);
                    EditorGUI.PropertyField(
                        position.SetHeight(relativePropertyHeight),
                        relativeProperty,
                        new GUIContent(langString)
                    );
                    position = position.AddY(relativePropertyHeight + SPACE);
                }
            }
        }

        private float GetAutoLocalizationIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.AutoLocalization)
                return 0;
            return LABEL_HEIGHT + SPACE;
        }

        private Rect DrawAutoLocalizationIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.AutoLocalization)
                return position;
            EditorGUI.LabelField(position.SetHeight(LABEL_HEIGHT), "> Auto Localization", green);
            return position.AddY(LABEL_HEIGHT + SPACE);
        }

        private float GetManualIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.Manual)
                return 0;
            return LABEL_HEIGHT + SPACE;
        }

        private Rect DrawManualIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.Manual)
                return position;
            EditorGUI.LabelField(position.SetHeight(LABEL_HEIGHT), "> Manual Localization", green);
            return position.AddY(LABEL_HEIGHT);
        }
        
        private float GetCsvIfAvailableHeight(SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.CSVFile)
                return 0;
            var height = LABEL_HEIGHT + SPACE;
            if (property.FindPropertyRelative("ru").propertyType != SerializedPropertyType.String)
                return height;
            height += (LABEL_HEIGHT + SPACE) * 2;
            return height;
        }

        private Rect DrawCsvIfAvailable(Rect position, SerializedProperty property, GUIContent label)
        {
            if (settings.translateMethod != TranslateMethod.CSVFile)
                return position;
            EditorGUI.LabelField(
                position.SetHeight(LABEL_HEIGHT),
                "> CSV Localization (support only string type)",
                green);
            position = position.AddY(LABEL_HEIGHT + SPACE);

            if (property.FindPropertyRelative("ru").propertyType != SerializedPropertyType.String)
                return position;

            var identity = property.FindPropertyRelative("identity");
            identity.stringValue = EditorGUI.TextField(
                position
                    .WidthPercent(0.65f)
                    .SetHeight(LABEL_HEIGHT),
                identity.stringValue);

            var helperPosition = position
                .RelativeX(0.65f)
                .AddX(SPACE*4)
                .WidthPercent(0.4f)
                .SetHeight(LABEL_HEIGHT);

            if (!string.IsNullOrEmpty(identity.stringValue))
                EditorGUI.LabelField(helperPosition, "ID Translate");
            else
                EditorGUI.LabelField(helperPosition, "ID Translate (necessarily)", red);

            position = position.AddY(LABEL_HEIGHT + SPACE);
            if (GUI.Button(position
                    .SetX(EditorGUI.indentLevel * 30 + SPACE)
                    .AddWidth(-EditorGUI.indentLevel * 15 - SPACE)
                    .WidthPercent(0.5f)
                    .SetHeight(LABEL_HEIGHT), "Import", button))
            {
                var transfers = CSVManager.ImportTransfersByKey(settings, identity.stringValue);
                if (transfers != null)
                    SetLanguages(property, transfers);
            }

            if (GUI.Button(position
                    .RelativeX(0.5f)
                    .AddX(EditorGUI.indentLevel * 30)
                    .AddWidth(-EditorGUI.indentLevel * 15 - SPACE)
                    .WidthPercent(0.5f)
                    .AddWidth(-SPACE)
                    .SetHeight(LABEL_HEIGHT), "Export", button))
            {
                CSVManager.SetIDLineFile(settings, identity.stringValue, GetLanguages(property));
            }

            return position.AddY(LABEL_HEIGHT + SPACE);
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