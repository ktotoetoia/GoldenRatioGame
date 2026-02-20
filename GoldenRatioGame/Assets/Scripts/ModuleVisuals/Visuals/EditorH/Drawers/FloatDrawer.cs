using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class FloatDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.floatValue));
            p.floatValue = EditorGUI.FloatField(r, label, p.floatValue);
        }
    }
}