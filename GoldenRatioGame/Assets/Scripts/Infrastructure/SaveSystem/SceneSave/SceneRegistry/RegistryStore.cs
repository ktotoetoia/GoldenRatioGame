using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.SaveSystem
{
    internal class RegistryStore
    {
        private readonly ConcurrentDictionary<string, RegistryEntry> _entries = new();
        
        public event Action<string> OnRegistered;
        public event Action<string> OnUnregistered;

        public bool AddEntry(string id, GameObject go, IStateSerializable serializer)
        {
            if (string.IsNullOrEmpty(id)) return false;

            if (go && TryGetIdByObject(go, out string existingId))
            {
                if (existingId != id)
                {
                    _entries.TryRemove(existingId, out _);
                }
            }

            _entries.AddOrUpdate(id,
                key => new RegistryEntry
                {
                    Id = key,
                    WeakGo = go != null ? new WeakReference<GameObject>(go) : null,
                    StateSerializer = serializer
                },
                (key, existing) =>
                {
                    existing.WeakGo = go != null ? new WeakReference<GameObject>(go) : existing.WeakGo;
                    existing.StateSerializer = serializer ?? existing.StateSerializer;
                    return existing;
                });

            OnRegistered?.Invoke(id);
            return true;
        }

        public bool TryRemove(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_entries.TryRemove(id, out _)) return false;
            OnUnregistered?.Invoke(id);
            return true;
        }
        
        public bool TryGetLiveGameObject(string id, out GameObject go)
        {
            go = null;
            if (!_entries.TryGetValue(id, out var e)) return false;

            if (e.WeakGo != null && e.WeakGo.TryGetTarget(out go)) 
            {
                if (go != null) return true; 
            }

            TryRemove(id); 
            go = null;
            return false;
        }
        
        public bool Contains(string id)
        {
            return _entries.TryGetValue(id, out var e) && 
                   e.WeakGo != null && 
                   e.WeakGo.TryGetTarget(out _);
        }

        public bool TryGetIdByObject(GameObject go, out string id)
        {
            id = null;
            if (go == null) return false;

            foreach (var kvp in _entries)
            {
                if (kvp.Value.WeakGo != null && 
                    kvp.Value.WeakGo.TryGetTarget(out var target) && 
                    target == go)
                {
                    id = kvp.Key;
                    return true;
                }
            }
            return false;
        }

        public KeyValuePair<string, RegistryEntry>[] Snapshot() => _entries.ToArray();

        public Dictionary<string, IStateSerializable> GetActiveSerializers()
        {
            return _entries
                .ToArray()
                .Select(kv => (kv.Key, serializer: kv.Value.GetSerializer()))
                .Where(t => t.serializer != null)
                .ToDictionary(t => t.Key, t => t.serializer);
        }

        public bool TryCleanupStaleAndRemove(string id)
        {
            if (!_entries.TryGetValue(id, out var e)) return false;
            if (e.WeakGo == null) return false;
            if (e.WeakGo.TryGetTarget(out _)) return false;

            _entries.TryRemove(id, out _);
            return true;
        }
    }
}