using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class EnumDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.enumIndex));
            p.intValue = EditorGUI.Popup(r, label, p.intValue, Enum.GetNames(type));
        }
    }
}