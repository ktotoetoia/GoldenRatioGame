using System;
using System.Collections.Generic;
using IM.Common;
using UnityEngine;

namespace IM.SaveSystem
{
    internal class GameObjectSyncer
    {
        private readonly RegistryStore _store;
        private readonly IPrefabResolver _defaultResolver;
        private readonly IGameObjectFactory _factory;

        public GameObjectSyncer(RegistryStore store, IPrefabResolver defaultResolver, IGameObjectFactory factory)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _defaultResolver = defaultResolver;
            _factory = factory;
        }

        public void ApplySavedObjects(IReadOnlyList<GameObjectData> savedObjects, IPrefabResolver resolver = null, bool instantiateMissing = true)
        {
            resolver ??= _defaultResolver;

            SyncGameObjects(savedObjects, resolver, instantiateMissing);
            RestoreStates(savedObjects, _store.GetActiveSerializers());
        }

        private void SyncGameObjects(IReadOnlyList<GameObjectData> dataList, IPrefabResolver resolver, bool instantiateMissing)
        {
            foreach (GameObjectData data in dataList)
            {
                if (!_store.TryGetLiveGameObject(data.Id, out GameObject existing) && instantiateMissing)
                {
                    SetupNewObject(data, resolver);
                }
            }
        }

        private void SetupNewObject(GameObjectData data, IPrefabResolver resolver)
        {
            GameObject prefab = !string.IsNullOrEmpty(data.PrefabId) ? resolver?.ResolvePrefab(data.PrefabId) : null;
            GameObject instance = prefab != null ? _factory.Create(prefab,true) : new GameObject($"placeholder-{data.Id}");

            if (!instance.TryGetComponent(out IIdentifiable ident))
            {
                GameObjectSerializer serializer = instance.AddComponent<GameObjectSerializer>();
                ident = serializer;
            }

            ident?.InjectId(data.Id);
            _store.AddEntry(ident.Id, instance, instance.GetComponent<IStateSerializable>());
        }

        private void RestoreStates(IReadOnlyList<GameObjectData> dataList, Dictionary<string, IStateSerializable> serializers)
        {
            foreach (GameObjectData data in dataList)
            {
                if (serializers.TryGetValue(data.Id, out IStateSerializable s) && s != null)
                {
                    try
                    {
                        s.Restore(data, x =>
                        {
                            if (_store.TryGetLiveGameObject(x, out GameObject go))
                            {
                                return go;
                            }

                            return null;
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Restore failed for {data.Id}");
                        Debug.LogException(ex);
                    }
                }
            }
        }
    }
}