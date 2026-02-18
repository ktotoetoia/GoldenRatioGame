using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    [CustomPropertyDrawer(typeof(AnimationChange))]
    public class AnimationChangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null) return;
            EditorGUI.BeginProperty(position, label, property);

            float lineH = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty typeProp = property.FindPropertyRelative("_animationChangeType");
            SerializedProperty nameProp = property.FindPropertyRelative("_parameterName");
            SerializedProperty boolProp = property.FindPropertyRelative("_boolValue");
            SerializedProperty intProp = property.FindPropertyRelative("_intValue");
            SerializedProperty floatProp = property.FindPropertyRelative("_floatValue");

            Rect rowRect = new Rect(position.x, position.y, position.width, lineH);
            rowRect = EditorGUI.PrefixLabel(rowRect, GUIUtility.GetControlID(FocusType.Passive), label);

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (typeProp != null)
                EditorGUI.PropertyField(new Rect(position.x, rowRect.y, position.width, lineH), typeProp, GUIContent.none);
            else
                EditorGUI.LabelField(new Rect(position.x, rowRect.y, position.width, lineH), "Missing _animationChangeType");

            Rect nameRect = new Rect(position.x, rowRect.y + lineH + spacing, position.width, lineH);
            if (nameProp != null)
                EditorGUI.PropertyField(nameRect, nameProp);
            else
                EditorGUI.LabelField(nameRect, "Missing _parameterName");

            ParameterType changeType = ParameterType.None;
            if (typeProp != null) changeType = (ParameterType)typeProp.enumValueIndex;

            Rect valueRect = new Rect(position.x, nameRect.y + lineH + spacing, position.width, lineH);

            switch (changeType)
            {
                case ParameterType.Bool:
                    if (boolProp != null) EditorGUI.PropertyField(valueRect, boolProp, new GUIContent("Value"));
                    else EditorGUI.LabelField(valueRect, "Missing _boolValue");
                    break;
                case ParameterType.Int:
                    if (intProp != null) EditorGUI.PropertyField(valueRect, intProp, new GUIContent("Value"));
                    else EditorGUI.LabelField(valueRect, "Missing _intValue");
                    break;
                case ParameterType.Float:
                    if (floatProp != null) EditorGUI.PropertyField(valueRect, floatProp, new GUIContent("Value"));
                    else EditorGUI.LabelField(valueRect, "Missing _floatValue");
                    break;
                case ParameterType.Trigger:
                case ParameterType.None:
                default:
                    break;
            }

            EditorGUI.indentLevel = oldIndent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property == null) return EditorGUIUtility.singleLineHeight;

            SerializedProperty typeProp = property.FindPropertyRelative("_animationChangeType");
            float lineH = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            int lines = 2;
            if (typeProp != null)
            {
                ParameterType changeType = (ParameterType)typeProp.enumValueIndex;
                if (changeType == ParameterType.Bool ||
                    changeType == ParameterType.Int ||
                    changeType == ParameterType.Float)
                {
                    lines += 1;
                }
            }

            return lines * lineH + (lines - 1) * spacing;
        }
    }
}