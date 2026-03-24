using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class IntDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.intValue));
            p.intValue = EditorGUI.IntField(r, label, p.intValue);
        }
    }
}