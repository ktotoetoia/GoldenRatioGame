using System;
using System.Collections.Generic;
using IM.Common;
using UnityEngine;

namespace IM.SaveSystem
{
    public class SceneRegistry : ISceneRegistry
    {
        private readonly RegistryStore _store;
        private readonly ScenePersistence _persistence;
        private readonly GameObjectSyncer _syncer;

        public event Action<string> OnRegistered;
        public event Action<string> OnUnregistered;
        
        public SceneRegistry(IGameObjectFactory factory = null, IPrefabResolver defaultResolver = null)
        {
            _store = new RegistryStore();
            _persistence = new ScenePersistence(_store);
            _syncer = new GameObjectSyncer(_store, defaultResolver?? new AddressablePrefabResolver(), factory?? new GameObjectDefaultFactory());

            _store.OnRegistered += id => OnRegistered?.Invoke(id);
            _store.OnUnregistered += id => OnUnregistered?.Invoke(id);
        }

        public void Deserialize(string json)
        {
            SceneSaveFile save = ScenePersistence.DeserializeToSaveFile(json);
            
            foreach (GameObjectData obj in save.Objects) _store.AddEntry(obj.Id, null, null);
            ApplySavedObjects(save.Objects);
        }

        public string Serialize() 
        {
            return _persistence.Serialize();
        }

        public void ApplySavedObjects(IReadOnlyList<GameObjectData> savedObjects, IPrefabResolver resolver = null, bool instantiateMissing = true)
            => _syncer.ApplySavedObjects(savedObjects, resolver, instantiateMissing);

        public bool Register(GameObject go)
        {
            return go && go.TryGetComponent(out IIdentifiable ident) &&
                   _store.AddEntry(ident.Id, go, go.GetComponent<IStateSerializable>());
        }

        public bool Unregister(GameObject go)
        {
            if (go == null) return false;
    
            if (go.TryGetComponent(out IIdentifiable ident)) return Unregister(ident.Id);
            if (_store.TryGetIdByObject(go, out string id)) return Unregister(id);
    
            return false;
        }
        public bool Unregister(string id)
        { 
            return _store.TryRemove(id);
        }

        public GameObject GetById(string id)
        {
            return _store.TryGetLiveGameObject(id, out GameObject g) ? g : null;
        }
        
        public bool Contains(string id) => _store.Contains(id);
        public bool Contains(GameObject go) => _store.TryGetIdByObject(go, out _);
        public Dictionary<string, IStateSerializable> GetActiveSerializers() => _store.GetActiveSerializers();
    }
}