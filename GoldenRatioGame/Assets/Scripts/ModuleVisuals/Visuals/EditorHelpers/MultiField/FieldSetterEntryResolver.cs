using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public class FieldSetterEntryResolver<T>
    {
        private readonly List<(IMemberValueProvider<T> entry, MemberAccessor accessor)> _resolved = new();
            
        public void Resolve(IEnumerable<IMemberValueProvider<T>> targets)
        {
            _resolved.Clear();
                
            foreach (IMemberValueProvider<T> e in targets)
            {
                if (e == null || e.TargetComponent == null) continue;
                MemberAccessor accessor = MemberAccessor.Create(e.TargetComponent, e.MemberName);
                _resolved.Add((e, accessor));
            }
        }

        public void OnValueChanged(T newValue)
        {
            foreach ((IMemberValueProvider<T> entry, MemberAccessor accessor) in _resolved)
            {
                if (accessor is not { IsWritable: true })
                {
                    Debug.LogWarning($"no writable member '{entry.MemberName}' on {entry.TargetComponent}");
                    continue;
                }

                SerializedValue sv = entry.GetT(newValue);
                object value = sv.GetValue(accessor.MemberType);

                try
                {
                    accessor.Set(entry.TargetComponent, value);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(entry.TargetComponent);
#endif
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($" failed to set '{entry.MemberName}' on {entry.TargetComponent}: {ex.Message}");
                }
            }
        }
    }
}