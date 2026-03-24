#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IM.Values
{
    [CustomPropertyDrawer(typeof(CappedValue<float>))]
    public class CappedFloatDrawer : PropertyDrawer
    {
    private const float FieldWidth = 40f;
    private const float Padding = 4f;
    private const float SmallSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty minProp = property.FindPropertyRelative("_minValue");
        SerializedProperty maxProp = property.FindPropertyRelative("_maxValue");
        SerializedProperty valueProp = property.FindPropertyRelative("_value");

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        (Rect minRect, Rect sliderRect, Rect maxRect) = GetRects(position);

        EditorGUI.BeginChangeCheck();

        EditorGUI.PropertyField(minRect, minProp, GUIContent.none);

        float min = minProp.floatValue;
        float max = maxProp.floatValue;

        if (min > max)
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUI.Slider(sliderRect, valueProp, min, min, GUIContent.none);
        }
        else
        {
            EditorGUI.Slider(sliderRect, valueProp, min, max, GUIContent.none);
        }

        EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);

        if (EditorGUI.EndChangeCheck())
        {
            if (minProp.floatValue > maxProp.floatValue)
            {
                (minProp.floatValue, maxProp.floatValue) = (maxProp.floatValue, minProp.floatValue);
            }

            valueProp.floatValue = Mathf.Clamp(valueProp.floatValue, minProp.floatValue, maxProp.floatValue);
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
    
    private  (Rect minRect, Rect sliderRect, Rect maxRect) GetRects(Rect position)
    {
        float sliderWidth = position.width - FieldWidth * 2 - Padding;
        Rect minRect = new Rect(position.x, position.y, FieldWidth, position.height);
        Rect sliderRect = new Rect(position.x + FieldWidth + SmallSpacing, position.y, sliderWidth, position.height);
        Rect maxRect = new Rect(position.x + position.width - FieldWidth, position.y, FieldWidth, position.height);
        return (minRect, sliderRect, maxRect);
    }
    }
}
#endif