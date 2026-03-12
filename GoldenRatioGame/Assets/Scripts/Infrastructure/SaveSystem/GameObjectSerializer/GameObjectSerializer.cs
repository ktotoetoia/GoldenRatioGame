using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.SaveSystem
{
    [DisallowMultipleComponent]
    public class GameObjectSerializer : MonoBehaviour, IIdentifiable, IStateSerializable, IHaveComponentRegistry
    {
        [SerializeField] protected AssetReferenceGameObject _assetReference;
        
        private IComponentSerializerRegistry _registry;
        
        public string Id { get; private set; }
        public void InjectId(string id) => Id = id;

        public IComponentSerializerRegistry Registry
        {
            get => _registry ??= new ComponentSerializerRegistry();
            set => _registry = value;
        }

        public virtual GameObjectData Capture()
        {
            GameObjectData data = new GameObjectData
            {
                Id = Id,
                PrefabId = _assetReference?.AssetGUID,
                ScenePath = gameObject.scene.path
            };

            var components = GetComponentsToSerialize();
            
            for (int i = 0; i < components.Count; i++)
            {
                TryCaptureComponent(components[i], i, data);
            }

            return data;
        }

        public virtual void Restore(GameObjectData data)
        {
            Id = data.Id;
            var components = GetComponentsToSerialize();

            foreach (ComponentData compData in data.Components)
            {
                TryRestoreComponent(compData, components);
            }
        }

        protected virtual IList<Component> GetComponentsToSerialize()
        {
            return GetComponents<Component>();
        }

        protected virtual void TryCaptureComponent(Component comp, int index, GameObjectData data)
        {
            if (!comp) return;

            object state = Registry.GetSerializerFor(comp.GetType())?.CaptureState(comp);
            if (state == null) return;

            data.Components.Add(new ComponentData
            {
                TypeAlias = TypeCache.GetAlias(comp.GetType()),
                ComponentIndex = index,
                SerializedState = state 
            });
        }

        protected virtual void TryRestoreComponent(ComponentData compData, IList<Component> components)
        {
            Type compType = TypeCache.GetType(compData.TypeAlias);
            if (compType == null) return;

            if (compData.ComponentIndex >= components.Count) return;
            
            Component comp = components[compData.ComponentIndex];
            if (!comp || comp.GetType() != compType) return;

            IComponentSerializer serializer = Registry.GetSerializerFor(compType);
            if (serializer == null) return;

            if (compData.SerializedState != null) 
                serializer.RestoreState(comp, compData.SerializedState);
        }
    }
}