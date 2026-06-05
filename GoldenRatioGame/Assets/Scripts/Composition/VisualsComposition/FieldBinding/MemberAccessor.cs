using System;
using System.Reflection;
using UnityEngine;

namespace IM.Visuals
{
    public sealed class MemberAccessor
    {
        public Type MemberType { get; }
        public bool IsWritable { get; }
        private readonly Action<Component, object> _setter;

        private MemberAccessor(Type memberType, Action<Component, object> setter)
        {
            MemberType = memberType;
            _setter = setter;
            IsWritable = setter != null;
        }

        public void Set(Component target, object value)
        {
            _setter?.Invoke(target, value);
        }

        public static MemberAccessor Create(Component target, string memberName)
        {
            var t = target.GetType();
            var field = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                var setter = new Action<Component, object>((comp, val) => field.SetValue(comp, val));
                return new MemberAccessor(field.FieldType, setter);
            }

            var prop = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanWrite)
            {
                var setter = new Action<Component, object>((comp, val) => prop.SetValue(comp, val));
                return new MemberAccessor(prop.PropertyType, setter);
            }

            return new MemberAccessor(null, null);
        }
    }
}