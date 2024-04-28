using System;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    [CustomPropertyDrawer(typeof(ConditionallyVisibleAttribute))]
    public class ConditionallyVisiblePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldDisplay(property))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUI.PropertyField(position, property, label, includeChildren: true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldDisplay(property) 
                ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true) 
                : 0;
        }

        private bool ShouldDisplay(SerializedProperty property)
        {
            var attr = (ConditionallyVisibleAttribute)attribute;
            var dependentProp = property.serializedObject.FindProperty(attr.PropertyName);

            switch (attr.Mode)
            {
                case VisibilityAttributeMode.Boolean:
                    return attr.Invert? !dependentProp.boolValue : dependentProp.boolValue;
                case VisibilityAttributeMode.Object:
                    switch (dependentProp.propertyType)
                    {
                        case SerializedPropertyType.Generic:
                            break;
                        case SerializedPropertyType.Integer:
                            break;
                        case SerializedPropertyType.Boolean:
                            break;
                        case SerializedPropertyType.Float:
                            break;
                        case SerializedPropertyType.String:
                            break;
                        case SerializedPropertyType.Color:
                            break;
                        case SerializedPropertyType.ObjectReference:
                            break;
                        case SerializedPropertyType.LayerMask:
                            break;
                        case SerializedPropertyType.Enum:
                            return attr.Value is Enum && (int)attr.Value == dependentProp.intValue; 
                        case SerializedPropertyType.Vector2:
                            break;
                        case SerializedPropertyType.Vector3:
                            break;
                        case SerializedPropertyType.Vector4:
                            break;
                        case SerializedPropertyType.Rect:
                            break;
                        case SerializedPropertyType.ArraySize:
                            break;
                        case SerializedPropertyType.Character:
                            break;
                        case SerializedPropertyType.AnimationCurve:
                            break;
                        case SerializedPropertyType.Bounds:
                            break;
                        case SerializedPropertyType.Gradient:
                            break;
                        case SerializedPropertyType.Quaternion:
                            break;
                        case SerializedPropertyType.ExposedReference:
                            break;
                        case SerializedPropertyType.FixedBufferSize:
                            break;
                        case SerializedPropertyType.Vector2Int:
                            break;
                        case SerializedPropertyType.Vector3Int:
                            break;
                        case SerializedPropertyType.RectInt:
                            break;
                        case SerializedPropertyType.BoundsInt:
                            break;
                        case SerializedPropertyType.ManagedReference:
                            break;
                        case SerializedPropertyType.Hash128:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return attr.Value == dependentProp.boxedValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}