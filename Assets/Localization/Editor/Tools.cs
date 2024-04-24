
using UnityEditor;
using UnityEngine;

namespace Localization
{
    public static class Tools
    {
        private const string LocalizationManagerPrefab = "Assets/Localization/Runtime/WorkingData/LocalizationManager.prefab";

        [MenuItem("Tools/Localization/Create Localization Manager")]
        public static void CreateLocalizationManager()
        {
            if (Object.FindAnyObjectByType<LocalizationManager>(FindObjectsInactive.Include) != null)
            {
                Debug.LogError($"{nameof(LocalizationManager)} is already exist!");
                return;
            }
            
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(LocalizationManagerPrefab);
            if (prefab == null)
            {
                Debug.LogError($"Cant find {nameof(LocalizationManager)} on \"{LocalizationManagerPrefab}\"");
                return;
            }
            
            var instantiate = PrefabUtility.InstantiatePrefab(prefab);
            EditorUtility.SetDirty(instantiate);
            Undo.RegisterCreatedObjectUndo(instantiate, "Create Localization Manager");
        }
    }
}
