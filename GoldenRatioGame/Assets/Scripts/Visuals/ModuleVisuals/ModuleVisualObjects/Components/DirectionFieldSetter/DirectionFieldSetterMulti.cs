using System;
using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class DirectionFieldSetterMulti : MonoBehaviour, IHorizontalDirectionDependant
    {
        [SerializeField] private List<TargetEntry> _entries = new ();
        private readonly List<(TargetEntry entry, MemberAccessor accessor)> _resolved = new();

#if UNITY_EDITOR
        private void OnValidate() => ResolveAllEntries();
#endif
        
        private void Awake() => ResolveAllEntries();

        private void ResolveAllEntries()
        {
            _resolved.Clear();
            
            foreach (TargetEntry e in _entries)
            {
                if (e == null || e.targetComponent == null) continue;
                MemberAccessor accessor = MemberAccessor.Create(e.targetComponent, e.memberName);
                _resolved.Add((e, accessor));
            }
        }
        
        public void OnDirectionChanged(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.None) return;

            foreach ((TargetEntry entry, MemberAccessor accessor) in _resolved)
            {
                if (accessor is not { IsWritable: true })
                {
                    Debug.LogWarning($"{name}: no writable member '{entry.memberName}' on {entry.targetComponent}");
                    continue;
                }

                SerializedValue sv = direction == HorizontalDirection.Left ? entry.left : entry.right;
                object value = sv.GetValue(accessor.MemberType);

                try
                {
                    accessor.Set(entry.targetComponent, value);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(entry.targetComponent);
#endif
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"{name}: failed to set '{entry.memberName}' on {entry.targetComponent}: {ex.Message}");
                }
            }
        }
    }
}