using System;
using IM.OdinSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.SaveSystem
{
    [DisallowMultipleComponent]
    public class GameObjectSerializer : MonoBehaviour, IIdentifiable, IStateSerializable
    {
        [SerializeField] private AssetReferenceGameObject  _assetReference;
        
        private readonly IComponentSerializerRegistry _registry = new ComponentSerializerRegistry();
        public string Id { get; private set; }

        public void InjectId(string id) => Id = id;

        public GameObjectData Capture()
        {
            
            GameObjectData data = new GameObjectData
            {
                Id = Id,
                PrefabId = _assetReference.AssetGUID,
                ScenePath = gameObject.scene.path
            };

            Component[] components = GetComponents<Component>();
            for (int i = 0; i < components.Length; i++) TryCaptureComponent(components[i], i, data);

            return data;
        }

        public void Restore(GameObjectData data)
        {
            Id = data.Id;
            Component[] components = GetComponents<Component>();

            foreach (ComponentData compData in data.Components) TryRestoreComponent(compData, components);
        }

        private void TryCaptureComponent(Component comp, int index, GameObjectData data)
        {
            if (!comp) return;

            object state = _registry.GetSerializerFor(comp.GetType())?.CaptureState(comp);
            if (state == null) return;

            data.Components.Add(new ComponentData
            {
                TypeAlias = TypeCache.GetAlias(comp.GetType()),
                ComponentIndex = index,
                SerializedState = state 
            });
        }

        private void TryRestoreComponent(ComponentData compData, Component[] components)
        {
            Type compType = TypeCache.GetType(compData.TypeAlias);
            if (compType == null) return;

            if (compData.ComponentIndex >= components.Length) return;
            Component comp = components[compData.ComponentIndex];
            if (!comp || comp.GetType() != compType) return;

            IComponentSerializer serializer = _registry.GetSerializerFor(compType);
            if (serializer == null) return;

            if (compData.SerializedState != null) serializer.RestoreState(comp, compData.SerializedState);
        }
    }
}