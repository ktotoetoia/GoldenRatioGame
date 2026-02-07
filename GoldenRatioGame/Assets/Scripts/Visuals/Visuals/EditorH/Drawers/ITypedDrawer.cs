using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public interface ITypedDrawer
    {
        void Draw(Rect r, string label, SerializedProperty prop, Type type);
    }
}