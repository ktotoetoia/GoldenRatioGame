using IM.Common;
using UnityEngine;

namespace IM.SaveSystem
{
    public class SerializationGameObjectObserver : MonoBehaviour, IGameObjectFactoryObserver
    {
        private readonly IComponentSerializerContainer _container = new ComponentSerializerContainer();
        private IHaveSceneRegistry _registrySource;
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            if (instance.TryGetComponent(out IIdentifiable identifiable))
            {
                (_registrySource ??= GetComponent<IHaveSceneRegistry>()).SceneRegistry.Register(instance);
            }

            if (instance.TryGetComponent(out IRequireComponentSerializerContainer componentRegistry))
            {
                componentRegistry.Container = _container;
            }
        }
    }
}