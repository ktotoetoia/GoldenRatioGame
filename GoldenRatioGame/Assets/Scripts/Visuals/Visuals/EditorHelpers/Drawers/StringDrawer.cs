using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class StringDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.stringValue));
            p.stringValue = EditorGUI.TextField(r, label, p.stringValue);
        }
    }
}