using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using TMPro;

namespace Localization
{
    public class AutoLocalizationMasse : EditorWindow
    {
        [MenuItem("Tools/Localization/Auto Localization Masse")]
        public static void ShowWindow()
        {
            GetWindow<AutoLocalizationMasse>("Auto Localization Masse");
        }

        Vector2 scrollPosition = Vector2.zero;
        List<GameObject> objectsTranlate = new List<GameObject>();
        private LocalizationSettings settings;
        private void OnEnable()
        {
            settings = SettingsLoader.LoadSettings();
        }

        private void OnGUI()
        {
            if (settings.translateMethod == TranslateMethod.AutoLocalization)
            {
                GUILayout.Space(10);
                
                if (GUILayout.Button("Search for all objects on the scene by type TEXT_MESH_PRO", GUILayout.Height(30)))
                {
                    objectsTranlate.Clear();

                    foreach (TMP_Text obj in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    {
                        objectsTranlate.Add(obj.gameObject);
                    }
                }
                
                if (GUILayout.Button($"Search for all objects on the scene by type {nameof(LanguageText)}", GUILayout.Height(30)))
                {
                    objectsTranlate.Clear();

                    foreach (var obj in FindObjectsByType<LanguageText>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    {
                        objectsTranlate.Add(obj.gameObject);
                    }
                }

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Add selected from list", GUILayout.Height(22)))
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        if (obj.GetComponent<TMP_Text>())
                        {
                            bool check = false;
                            for (int i = 0; i < objectsTranlate.Count; i++)
                                if (obj == objectsTranlate[i])
                                    check = true;

                            if (!check)
                                objectsTranlate.Add(obj);
                        }
                    }
                }

                if (GUILayout.Button("Remove selected from list", GUILayout.Height(22)))
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        objectsTranlate.Remove(obj);
                    }
                }

                if (objectsTranlate.Count > 0)
                {
                    if (GUILayout.Button("Clear list", GUILayout.Height(22)))
                    {
                        objectsTranlate.Clear();
                    }
                }

                GUILayout.EndHorizontal();

                if (objectsTranlate.Count > 0)
                {
                    GUILayout.Space(10);

                    if (GUILayout.Button("TRANSLATE", GUILayout.Height(30)))
                    {
                        foreach (GameObject obj in objectsTranlate)
                        {
                            LanguageText scrAL = obj.GetComponent<LanguageText>();

                            if (scrAL == null)
                                scrAL = obj.AddComponent<LanguageText>();

                            scrAL.Serialize();
                            scrAL.componentTextField = true;
                            scrAL.Translate(19);
                        }
                    }

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button($"Remove component {nameof(LanguageText)}", GUILayout.Height(22)))
                    {
                        foreach (GameObject obj in objectsTranlate)
                        {
                            LanguageText scrAL = obj.GetComponent<LanguageText>();

                            if (scrAL)
                                DestroyImmediate(scrAL);
                        }
                    }

                    if (GUILayout.Button($"Reserialize {nameof(LanguageText)} components", GUILayout.Height(22)))
                    {
                        foreach (GameObject obj in objectsTranlate)
                        {
                            LanguageText scrAL = obj.GetComponent<LanguageText>();

                            if (scrAL)
                                scrAL.Serialize();
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Label($"({objectsTranlate.Count} objects in the list)", style, GUILayout.ExpandWidth(true));

                if (objectsTranlate.Count > 10 && position.height < objectsTranlate.Count * 20.6f + 190)
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Height(position.height - 190));

                for (int i = 0; i < objectsTranlate.Count; i++)
                {
                    objectsTranlate[i] = (GameObject)EditorGUILayout.ObjectField($"{i + 1}. {objectsTranlate[i].name}", objectsTranlate[i], typeof(GameObject), false);
                }

                if (objectsTranlate.Count > 10 && position.height < objectsTranlate.Count * 20.6f + 190)
                    GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Select Auto Location Inspector in the plugin settings\nInfoYG -> Translate Metod -> AutoLocalization", GUILayout.ExpandWidth(true));
            }

            if (GUI.changed && objectsTranlate.Count > 0)
            {
                EditorUtility.SetDirty(objectsTranlate[0].gameObject);
                EditorSceneManager.MarkSceneDirty(objectsTranlate[0].gameObject.scene);
            }
        }
    }
}

