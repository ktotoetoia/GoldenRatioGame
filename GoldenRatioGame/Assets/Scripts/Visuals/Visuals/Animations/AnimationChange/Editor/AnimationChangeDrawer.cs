using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
   [CustomPropertyDrawer(typeof(AnimationChange))]
    public class AnimationChangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty typeProp = property.FindPropertyRelative("_animationChangeType");
            SerializedProperty nameProp = property.FindPropertyRelative("_propertyName");
            SerializedProperty boolProp = property.FindPropertyRelative("_boolValue");
            SerializedProperty intProp = property.FindPropertyRelative("_intValue");
            SerializedProperty floatProp = property.FindPropertyRelative("_floatValue");

            float lineH = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect rect = new Rect(position.x, position.y, position.width, lineH);

            rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(new Rect(position.x, rect.y, position.width, lineH), typeProp);

            Rect nameRect = new Rect(position.x, rect.y + lineH + spacing, position.width, lineH);
            EditorGUI.PropertyField(nameRect, nameProp);

            Rect valueRect = new Rect(position.x, nameRect.y + lineH + spacing, position.width, lineH);

            AnimationChangeType changeType = (AnimationChangeType)typeProp.enumValueIndex;

            switch (changeType)
            {
                case AnimationChangeType.Bool:
                    EditorGUI.PropertyField(valueRect, boolProp, new GUIContent("Value"));
                    break;
                case AnimationChangeType.Int:
                    EditorGUI.PropertyField(valueRect, intProp, new GUIContent("Value"));
                    break;
                case AnimationChangeType.Float:
                    EditorGUI.PropertyField(valueRect, floatProp, new GUIContent("Value"));
                    break;
                case AnimationChangeType.Trigger:
                case AnimationChangeType.None:
                default:
                    break;
            }

            EditorGUI.indentLevel = oldIndent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property == null)
                return EditorGUIUtility.singleLineHeight;

            SerializedProperty typeProp =
                property.FindPropertyRelative("_animationChangeType");

            if (typeProp == null)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }

            float lineH = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            int lines = 2;

            AnimationChangeType changeType =
                (AnimationChangeType)typeProp.enumValueIndex;

            if (changeType == AnimationChangeType.Bool ||
                changeType == AnimationChangeType.Int ||
                changeType == AnimationChangeType.Float)
            {
                lines += 1;
            }

            return lines * lineH + (lines - 1) * spacing;
        }
    }
}