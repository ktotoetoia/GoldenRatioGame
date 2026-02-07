using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class BoolDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.boolValue));
            p.boolValue = EditorGUI.Toggle(r, label, p.boolValue);
        }
    }
}