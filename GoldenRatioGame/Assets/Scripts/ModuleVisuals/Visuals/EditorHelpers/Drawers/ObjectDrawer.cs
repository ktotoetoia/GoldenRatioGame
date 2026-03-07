using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class ObjectDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            SerializedProperty p = prop.FindPropertyRelative(nameof(SerializedValue.objectValue));
            p.objectReferenceValue = EditorGUI.ObjectField(r, label, p.objectReferenceValue, type, true);
        }
    }
}