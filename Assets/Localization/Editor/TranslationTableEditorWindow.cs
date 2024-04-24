using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Localization
{
    public class TranslationTableEditorWindow : EditorWindow
    {
        private bool enabled;
        public static void ShowWindow()
        {
            GetWindow<TranslationTableEditorWindow>("Translation Table");
        }

        Vector2 scrollPosition = Vector2.zero;

        private void OnEnable()
        {
            enabled = true;
            TextAsset data = Resources.Load(LocalizationManager.Settings.CSVFileTranslate.name) as TextAsset;
            if (data == null)
            {
                Debug.LogError("CSV file was not found!");
                enabled = false;
            }
        }

        private void OnDisable()
        {
            enabled = false;
        }

        private void OnGUI()
        {
            if (!enabled) return;
            
            int countSelectObj = 0;

            foreach (GameObject obj in Selection.gameObjects)
            {
                countSelectObj++;
                var lang = obj.GetComponent<LanguageText>();

                if (lang)
                {
                    var data = Resources.Load(lang.Settings.CSVFileTranslate.name) as TextAsset;
                    var lines = Regex.Split(CSVManager.CommaFormat(data.text), "\n");

                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));
                    GUILayout.BeginHorizontal();
                    var keys = Regex.Split(lines[0], ",");

                    for (var i = 0; i < keys.Length; i++)
                    {
                        GUILayout.BeginVertical("Box");
                        if (i == 0) GUILayout.Label(keys[i].Replace("*", ",").Replace(@"\n", "\n"), GUILayout.Width(108), GUILayout.Height(15));
                        else GUILayout.Label(keys[i].Replace("*", ",").Replace(@"\n", "\n"), GUILayout.Width(100), GUILayout.Height(15));
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();

                    for (int i = 1; i < lines.Length - 1; i++)
                    {
                        DrawLine(lines[i], lang);
                    }
                }
                else
                {
                    this.Close();
                }
            }

            if (countSelectObj < 1)
            {
                this.Close();
            }

            GUILayout.EndScrollView();
        }

        void DrawLine(string line, LanguageText lang)
        {
            GUILayout.BeginHorizontal();

            string[] keys = Regex.Split(line, ",");

            for (int i = 0; i < keys.Length; i++)
            {
                if (i == 0)
                {

                    if (GUILayout.Button(keys[i].Replace("*", ",").Replace(@"\n", "\n"), GUILayout.Width(120), GUILayout.Height(26)))
                    {
                        lang.text = keys[i];
                    }
                }
                else
                {
                    GUILayout.BeginVertical("HelpBox");
                    GUILayout.Label(keys[i].Replace("*", ",").Replace(@"\n", "\n"), GUILayout.Width(100), GUILayout.Height(20));
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
