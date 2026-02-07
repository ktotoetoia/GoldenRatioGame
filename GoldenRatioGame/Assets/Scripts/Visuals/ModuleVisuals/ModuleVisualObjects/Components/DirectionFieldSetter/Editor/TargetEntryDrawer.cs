using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    [CustomPropertyDrawer(typeof(TargetEntry))]
    public class TargetEntryDrawer : PropertyDrawer
    {
        private readonly TypedDrawerRegistry _registry = new();
        private const float Pad = 2f;
        private static float Line => EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (Line + Pad) * 6;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            RowLayout layout = new RowLayout(position, Line, Pad);

            DrawHeader(layout.Next(), label);

            SerializedProperty compProp = property.FindPropertyRelative(nameof(TargetEntry.targetComponent));
            SerializedProperty nameProp = property.FindPropertyRelative(nameof(TargetEntry.memberName));
            SerializedProperty leftProp = property.FindPropertyRelative(nameof(TargetEntry.left));
            SerializedProperty rightProp = property.FindPropertyRelative(nameof(TargetEntry.right));

            DrawComponent(layout.Next(), compProp);

            Type selectedType = DrawMember(layout.Next(), compProp, nameProp);

            DrawValues(layout.Next(), layout.Next(), leftProp, rightProp, selectedType);

            EditorGUI.EndProperty();
        }

        private void DrawHeader(Rect r, GUIContent label)
        {
            EditorGUI.LabelField(r, label, EditorStyles.boldLabel);
        }

        private void DrawComponent(Rect r, SerializedProperty compProp)
        {
            EditorGUI.PropertyField(r, compProp);
        }

        private Type DrawMember(Rect r, SerializedProperty compProp, SerializedProperty nameProp)
        {
            Component comp = compProp.objectReferenceValue as Component;

            if (comp == null)
            {
                EditorGUI.PropertyField(r, nameProp);
                return null;
            }

            MemberCollection members = CollectMembers(comp.GetType());

            int index = Mathf.Max(0, members.Keys.IndexOf(nameProp.stringValue));
            int chosen = EditorGUI.Popup(r, "Member", index, members.DisplayNames);

            nameProp.stringValue = members.Keys[chosen];

            return members.Types[chosen];
        }

        private void DrawValues(Rect leftRect, Rect rightRect,
            SerializedProperty leftProp,
            SerializedProperty rightProp,
            Type type)
        {
            if (type == null)
            {
                EditorGUI.PropertyField(leftRect, leftProp, true);
                EditorGUI.PropertyField(rightRect, rightProp, true);
                return;
            }

            _registry.Get(type).Draw(leftRect,"Left",leftProp,type);
            _registry.Get(type).Draw(rightRect,"Right",rightProp,type);
        }

        private MemberCollection CollectMembers(Type type)
        {
            List<string> keys = new List<string>();
            List<string> names = new List<string>();
            List<Type> types = new List<Type>();

            foreach (FieldInfo f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                keys.Add(f.Name);
                names.Add($"{f.Name} (Field) : {f.FieldType.Name}");
                types.Add(f.FieldType);
            }

            foreach (PropertyInfo p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!p.CanWrite) continue;

                keys.Add(p.Name);
                names.Add($"{p.Name} (Prop) : {p.PropertyType.Name}");
                types.Add(p.PropertyType);
            }

            return new MemberCollection(keys, names.ToArray(), types);
        }
        
        private readonly struct MemberCollection
        {
            public readonly List<string> Keys;
            public readonly string[] DisplayNames;
            public readonly List<Type> Types;

            public MemberCollection(List<string> keys, string[] displayNames, List<Type> types)
            {
                Keys = keys;
                DisplayNames = displayNames;
                Types = types;
            }
        }
    }
}