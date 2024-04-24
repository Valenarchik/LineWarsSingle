// using LineWars.Model;
// using UnityEditor;
// using UnityEngine;
//
// namespace LineWars
// {
//     public static class Tools
//     {
//         [MenuItem("Tools/Method")]
//         public static void Method()
//         {
//             foreach (var guid in AssetDatabase.FindAssets("t:GameObject", new []{"Assets/LineWars/Units"}))
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(guid);
//                 var gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//                 var unit = gameObject.GetComponent<Unit>();
//                 
//                 if (unit == null)
//                     continue;
//                 
//                 unit.langUnitName.ru = unit.unitName;
//                 unit.langUnitDescription.ru = unit.unitDescription;
//                 
//                 EditorUtility.SetDirty(unit);
//             }
//         }
//     }
// }