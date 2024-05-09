using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

namespace Localization
{
    [CustomEditor(typeof(LanguageText))]
    [CanEditMultipleObjects]
    public class LanguageTextEditor : Editor
    {
        private const string buttonText_ReplaseFont = "Replace the font with the standard one";
        private LanguageText scr;

        private GUIStyle red;
        private GUIStyle green;
        private int processTranslateLabel;

        private void OnEnable()
        {
            scr = (LanguageText)target;
            scr.Serialize();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            scr = (LanguageText)target;
            Undo.RecordObject(scr, "Undo LanguageYG");

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

            var isNullTextComponent = scr.textMPComponent == null;


            if (isNullTextComponent)
            {
                if (GUILayout.Button("Add component - Text Mesh Pro UGUI", GUILayout.Height(23)))
                {
                    scr.textMPComponent = scr.gameObject.AddComponent<TextMeshProUGUI>();
                }
                return;
            }

            if (scr.Settings)
            {
                if (scr.Settings.translateMethod == TranslateMethod.CSVFile)
                {
                    GUILayout.BeginVertical("HelpBox");

                    scr.componentTextField = EditorGUILayout.ToggleLeft("Component Text/TextMeshPro Translate", scr.componentTextField);

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(">", GUILayout.Width(20)))
                    {
                        TranslationTableEditorWindow.ShowWindow();
                    }

                    var availableStr = true;

                    if (scr.componentTextField)
                    {
                        if (scr.textMPComponent)
                        {
                            GUILayout.Label(scr.textMPComponent.text);

                            if (scr.textMPComponent == null || scr.textMPComponent.text.Length == 0)
                                availableStr = false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(scr.text))
                            availableStr = false;

                        scr.text = EditorGUILayout.TextField(scr.text, GUILayout.Height(20));
                    }

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
                        var translfers = CSVManager.ImportTransfersByKey(scr);
                        if (translfers != null)
                            scr.languages = CSVManager.ImportTransfersByKey(scr);
                    }
                    if (GUILayout.Button("Export"))
                    {
                        CSVManager.SetIDLineFile(scr.Settings, scr);
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    
                    UpdateLanguages();
                }
                else
                {
                    if (scr.Settings.translateMethod == TranslateMethod.AutoLocalization)
                    {
                        GUILayout.BeginVertical("HelpBox");

                        scr.componentTextField = EditorGUILayout.ToggleLeft("Component Text/TextMeshPro Translate", scr.componentTextField);

                        if (!scr.componentTextField)
                            scr.text = EditorGUILayout.TextArea(scr.text, GUILayout.Height(scr.textHeight));
                        else
                        { 
                            if (scr.textMPComponent)
                                GUILayout.Label(scr.textMPComponent.text);
                        }

                        GUILayout.BeginHorizontal();

                        if (scr.componentTextField)
                        { 
                            if (scr.textMPComponent)
                            {
                                if (scr.textMPComponent.text != null && scr.textMPComponent.text.Length > 0)
                                {
                                    GUILayout.Label("TextMeshPro Component", green);

                                    if (GUILayout.Button("TRANSLATE"))
                                        TranslateButton();
                                }
                                else
                                    GUILayout.Label("TextMeshPro Component", red);
                            }
                        }
                        else
                        {
                            if (scr.componentTextField || string.IsNullOrEmpty(scr.text))
                            {
                                GUILayout.Label("FILL IN THE FIELD", red);
                            }
                            else if (GUILayout.Button("TRANSLATE"))
                                TranslateButton();
                        }

                        if (GUILayout.Button("CLEAR"))
                        {


                            scr.processTranslateLabel = "";
                            scr.countLang = processTranslateLabel;
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    GUILayout.BeginVertical("box");
                    GUILayout.BeginHorizontal();

                    var labelProcess = false;

                    if (scr.Settings.translateMethod == TranslateMethod.AutoLocalization)
                    {
                        if (scr.processTranslateLabel != "")
                        {
                            if (scr.countLang == processTranslateLabel)
                            {
                                GUILayout.Label(scr.processTranslateLabel, green, GUILayout.Height(20));
                                labelProcess = true;
                            }
                            else if (scr.processTranslateLabel == "")
                            {
                                labelProcess = true;
                            }
                            else
                            {
                                GUILayout.Label(scr.processTranslateLabel, GUILayout.Height(20));
                                labelProcess = true;
                            }
                            
                            if (scr.processTranslateLabel != null && scr.processTranslateLabel.Contains("error"))
                            {
                                GUILayout.Label(scr.processTranslateLabel, red, GUILayout.Height(20));
                                labelProcess = true;
                            }
                        }
                    }

                    if (labelProcess == false)
                        GUILayout.Label(processTranslateLabel + " Languages", GUILayout.Height(20));

 
                    if (scr.processTranslateLabel != null && !scr.processTranslateLabel.Contains("completed"))
                        GUILayout.Label("Go back to the inspector!", GUILayout.Height(20));
                    

                    GUILayout.EndHorizontal();

                    UpdateLanguages();
                    GUILayout.EndVertical();
                }
            }

            if (scr.textMPComponent)
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical("box");
                if (scr.textMPComponent)
                {
                    scr.uniqueFontTMP = (TMP_FontAsset)EditorGUILayout.ObjectField("Unique Font", scr.uniqueFontTMP, typeof(TMP_FontAsset), false);
                    FontTMPSettingsDraw();
                }
                GUILayout.EndVertical();
            }

            if (GUI.changed && !Application.isPlaying)
            {
                EditorUtility.SetDirty(scr.gameObject);
                EditorSceneManager.MarkSceneDirty(scr.gameObject.scene);
            }
        }
        
        void FontTMPSettingsDraw()
        {
            if (scr.Settings.fonts.defaultFont.Length == 0)
                return;

            scr.fontNumber = Mathf.Clamp(scr.fontNumber, 0, scr.Settings.fonts.defaultFont.Length - 1);
            if (scr.Settings.fonts.defaultFont.Length > 1)
                scr.fontNumber = EditorGUILayout.IntField("Font Index (in array default fonts)", scr.fontNumber);

            if (scr.textMPComponent.font == scr.Settings.fonts.defaultFont[scr.fontNumber])
                return;

            if (GUILayout.Button(buttonText_ReplaseFont))
            {
                Undo.RecordObject(scr.textMPComponent, "Undo TextMPComponent.font");
                scr.textMPComponent.font = scr.Settings.fonts.defaultFont[scr.fontNumber];
            }
        }

        private void TranslateButton()
        {
            scr.processTranslateLabel = "";
            scr.Translate(processTranslateLabel);
        }

        private void UpdateLanguages()
        {
            serializedObject.Update();
            processTranslateLabel = 0;
            scr.textHeight = EditorGUILayout.Slider("Text Height", scr.textHeight, 20f, 400f);
            var langArr = LangMethods.GetLangArr(scr.Settings);
            for (var i = 0; i < langArr.Length; i++)
            {
                if (!langArr[i]) continue;
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(LangMethods.GetLangName(i), LangMethods.FullNameLanguages()[i]), GUILayout.Width(20), GUILayout.Height(20));
                var property = serializedObject.FindProperty(LangMethods.GetLangName(i));
                property.stringValue = EditorGUILayout.TextArea(
                    property.stringValue,
                    EditorStyles.textArea, GUILayout.Height(scr.textHeight)
                );
                GUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
