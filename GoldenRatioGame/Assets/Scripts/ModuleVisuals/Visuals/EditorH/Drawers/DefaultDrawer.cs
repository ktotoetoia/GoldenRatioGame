using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class DefaultDrawer : ITypedDrawer
    {
        public void Draw(Rect r, string label, SerializedProperty prop, Type type)
        {
            EditorGUI.PropertyField(r, prop, true);
        }
    }
}