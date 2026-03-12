using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IM.OdinSerializer;
using UnityEngine;

namespace IM.SaveSystem
{
    public class SceneRegistry
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, Entry> _entries = new();
        private IPrefabResolver _defaultResolver = new AddressablePrefabResolver();
        
        public event Action<string> OnRegistered;
        public event Action<string> OnUnregistered;

        public static SceneRegistry Deserialize(string json)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            SceneSaveFile save = SerializationUtility.DeserializeValue<SceneSaveFile>(bytes, DataFormat.JSON);
            
            SceneRegistry registry = new SceneRegistry();
            foreach (GameObjectData obj in save.Objects) registry.AddEntry(obj.Id, null, null);
            return registry;
        }

        public string Serialize()
        {
            SceneSaveFile saveFile = CreateSaveFileFromRegistry();
            byte[] bytes = SerializationUtility.SerializeValue(saveFile, DataFormat.JSON);
            return Encoding.UTF8.GetString(bytes);
        }

        public void ApplySavedObjects(IEnumerable<GameObjectData> savedObjects, IPrefabResolver resolver = null, bool instantiateMissing = true)
        {
            List<GameObjectData> dataList = savedObjects.ToList();
            resolver ??= _defaultResolver;
            
            SyncGameObjects(dataList, resolver, instantiateMissing);

            Dictionary<string, IStateSerializable> serializers = GetActiveSerializers();
            InjectDependencies(dataList, serializers);

            RestoreStates(dataList, serializers);
        }

        public bool Register(GameObject go)
        {
            Debug.Log(go + "  " + go.TryGetComponent(out IIdentifiable _d) + "  ");
            
            return go != null && go.TryGetComponent(out IIdentifiable ident) && AddEntry(ident.Id, go, go.GetComponent<IStateSerializable>());
        }

        public bool Unregister(string id)
        {
            lock (_lock) { if (!_entries.Remove(id)) return false; }
            OnUnregistered?.Invoke(id);
            return true;
        }

        public GameObject GetById(string id)
        {
            lock (_lock) return _entries.TryGetValue(id, out Entry e) && e.WeakGo != null && e.WeakGo.TryGetTarget(out GameObject go) ? go : null;
        }
        
        private SceneSaveFile CreateSaveFileFromRegistry()
        {
            SceneSaveFile save = new SceneSaveFile();
            List<Entry> snapshot;
            lock (_lock) snapshot = _entries.Values.ToList();

            foreach (Entry e in snapshot)
            {
                if (e.GetSerializer() is { } s)
                {
                    try { save.Objects.Add(s.Capture()); }
                    catch (Exception ex) { Debug.LogError($"Capture failed for {e.Id}: {ex}"); }
                }
            }
            return save;
        }

        private void SyncGameObjects(List<GameObjectData> dataList, IPrefabResolver resolver, bool instantiateMissing)
        {
            foreach (GameObjectData data in dataList)
            {
                GameObject existing = GetById(data.Id);
                if (existing == null && instantiateMissing)
                {
                    SetupNewObject(data, resolver);
                }
            }
        }

        private void SetupNewObject(GameObjectData data, IPrefabResolver resolver)
        {
            GameObject prefab = !string.IsNullOrEmpty(data.PrefabId) ? resolver?.ResolvePrefab(data.PrefabId) : null;
            GameObject instance = prefab != null ? UnityEngine.Object.Instantiate(prefab) : new GameObject($"placeholder-{data.Id}");

            if (instance.TryGetComponent<IIdentifiable>(out IIdentifiable ident) || 
                instance.AddComponent<GameObjectSerializer>() is IIdentifiable identNew && (ident = identNew) != null)
            {
                ident.InjectId(data.Id);
            }

            Register(instance);
        }

        private Dictionary<string, IStateSerializable> GetActiveSerializers()
        {
            lock (_lock) return _entries.ToDictionary(k => k.Key, v => v.Value.GetSerializer());
        }

        private void InjectDependencies(List<GameObjectData> dataList, Dictionary<string, IStateSerializable> serializers)
        {
            foreach (GameObjectData data in dataList)
            {
                if (serializers.TryGetValue(data.Id, out IStateSerializable s) && s is IDependencyProvider prov && s is IDependencyReceiver recv)
                {
                    Dictionary<string, GameObject> deps = prov.GetDependencyIds().ToDictionary(id => id, id => 
                        serializers.TryGetValue(id, out IStateSerializable ds) ? (ds as MonoBehaviour)?.gameObject : null);
                    recv.InjectDependencies(deps);
                }
            }
        }

        private void RestoreStates(List<GameObjectData> dataList, Dictionary<string, IStateSerializable> serializers)
        {
            foreach (GameObjectData data in dataList)
            {
                if (serializers.TryGetValue(data.Id, out IStateSerializable s) && s != null)
                {
                    try { s.Restore(data); }
                    catch (Exception ex) { Debug.LogError($"Restore failed for {data.Id}: {ex}"); }
                }
            }
        }

        private bool AddEntry(string id, GameObject go, IStateSerializable serializer)
        {
            if (string.IsNullOrEmpty(id)) return false;
    
            lock (_lock)
            {
                if (_entries.TryGetValue(id, out var existing))
                {
                    if (existing.WeakGo != null && existing.WeakGo.TryGetTarget(out _))
                    {
                        return false;
                    }
            
                    existing.WeakGo = go != null ? new WeakReference<GameObject>(go) : null;
                    existing.StateSerializer = serializer;
                }
                else
                {
                    _entries[id] = new Entry { 
                        Id = id, 
                        WeakGo = go != null ? new WeakReference<GameObject>(go) : null, 
                        StateSerializer = serializer 
                    };
                }
            }
            OnRegistered?.Invoke(id);
            return true;
        }
        
        private class Entry
        {
            public string Id;
            public WeakReference<GameObject> WeakGo;
            public IStateSerializable StateSerializer;

            public IStateSerializable GetSerializer() => StateSerializer ?? 
                                                         (WeakGo != null && WeakGo.TryGetTarget(out GameObject go) ? go.GetComponent<IStateSerializable>() : null);
        }
    }
}