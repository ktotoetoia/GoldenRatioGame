using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IM.Visuals
{
    public class EntryDrawerBase : PropertyDrawer
    {
        private readonly TypedDrawerRegistry _registry = new();
        protected const float Pad = 2f;
        protected static float Line => EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (Line + Pad) * 6;
        }

        protected void DrawHeader(Rect r, GUIContent label)
        {
            EditorGUI.LabelField(r, label, EditorStyles.boldLabel);
        }

        protected void DrawComponent(Rect r, SerializedProperty compProp)
        {
            EditorGUI.PropertyField(r, compProp);
        }

        protected Type DrawMember(Rect r, SerializedProperty compProp, SerializedProperty nameProp)
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

        protected void DrawValue(Rect rect, SerializedProperty leftProp, Type type, string name)
        {
            if (type == null)
            {
                EditorGUI.PropertyField(rect, leftProp, true);
                return;
            }

            _registry.Get(type).Draw(rect, name, leftProp, type);
        }

        private MemberCollection CollectMembers(Type type)
        {
            List<string> keys = new List<string>();
            List<string> names = new List<string>();
            List<Type> types = new List<Type>();

            foreach (FieldInfo f in
                     type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                keys.Add(f.Name);
                names.Add($"{f.Name} (Field) : {f.FieldType.Name}");
                types.Add(f.FieldType);
            }

            foreach (PropertyInfo p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                          BindingFlags.NonPublic))
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