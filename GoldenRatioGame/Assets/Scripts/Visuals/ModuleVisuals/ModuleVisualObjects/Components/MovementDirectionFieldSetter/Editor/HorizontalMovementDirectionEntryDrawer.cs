using System;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    [CustomPropertyDrawer(typeof(MovementDirectionEntry))]
    public class HorizontalMovementDirectionEntryDrawer : EntryDrawerBase
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            RowLayout layout = new RowLayout(position, Line, Pad);

            DrawHeader(layout.Next(), label);

            SerializedProperty compProp = property.FindPropertyRelative(nameof(MovementDirectionEntry.targetComponent));
            SerializedProperty nameProp = property.FindPropertyRelative(nameof(MovementDirectionEntry.memberName));
            SerializedProperty leftProp = property.FindPropertyRelative(nameof(MovementDirectionEntry.left));
            SerializedProperty rightProp = property.FindPropertyRelative(nameof(MovementDirectionEntry.right));
            
            DrawComponent(layout.Next(), compProp);

            Type selectedType = DrawMember(layout.Next(), compProp, nameProp);

            DrawValue(layout.Next(), leftProp, selectedType, "Left");
            DrawValue(layout.Next(), rightProp, selectedType, "Right");

            EditorGUI.EndProperty();
        }
    }
}