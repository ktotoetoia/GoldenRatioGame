using System;
using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.SaveSystem
{
    [DisallowMultipleComponent]
    public class GameObjectSerializer : MonoBehaviour, IIdentifiable, IStateSerializable, IRequireComponentSerializerContainer
    {
        [SerializeField] protected AssetReferenceGameObject _assetReference;
        private IComponentSerializerContainer _container;
        
        public string Id { get; private set; }
        public void InjectId(string id) => Id = id;

        public IComponentSerializerContainer Container
        {
            get => _container ??= new ComponentSerializerContainer();
            set => _container = value;
        }

        public virtual GameObjectData Capture()
        {
            GameObjectData data = new GameObjectData
            {
                Id = Id,
                PrefabId = _assetReference?.AssetGUID,
            };
            
            IList<Component> components = GetComponentsToSerialize();
            
            for (int i = 0; i < components.Count; i++)
            {
                TryCaptureComponent(components[i], i, data);
            }
            
            return data;
        }

        public virtual void Restore(GameObjectData data, Func<string, GameObject> resolveDependency)
        {
            Id = data.Id;
            IList<Component> components = GetComponentsToSerialize();

            foreach (ComponentData compData in data.Components)
            {
                TryRestoreComponent(compData, components,resolveDependency);
            }
        }

        protected virtual IList<Component> GetComponentsToSerialize()
        {
            return GetComponents<Component>();
        }

        protected virtual void TryCaptureComponent(Component comp, int index, GameObjectData data)
        {
            if (!comp) return;

            object state = Container.GetSerializerFor(comp.GetType())?.CaptureState(comp);
            if (state == null) return;

            data.Components.Add(new ComponentData
            {
                TypeAlias = TypeCache.GetAlias(comp.GetType()),
                ComponentIndex = index,
                SerializedState = state 
            });
        }

        protected virtual void TryRestoreComponent(ComponentData compData, IList<Component> components, Func<string, GameObject> resolveDependency)
        {
            Type compType = TypeCache.GetType(compData.TypeAlias);
            if (compType == null) return;

            if (compData.ComponentIndex >= components.Count) return;
            
            Component comp = components[compData.ComponentIndex];
            if (!comp || comp.GetType() != compType) return;

            IComponentSerializer serializer = Container.GetSerializerFor(compType);
            if (serializer == null) return;

            if (compData.SerializedState != null) 
                serializer.RestoreState(comp, compData.SerializedState,resolveDependency);
        }
    }
}