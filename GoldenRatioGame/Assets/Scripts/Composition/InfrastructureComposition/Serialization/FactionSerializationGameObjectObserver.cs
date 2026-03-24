using IM.Factions;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class FactionSerializationGameObjectObserver : MonoBehaviour, IGameObjectFactoryObserver
    {
        [SerializeField] private FactionDatabase _factionDatabase;
        private IComponentSerializerContainer _container;
        private IHaveSceneRegistry _registrySource;

        private IComponentSerializerContainer Container => _container ??=
            new ComponentSerializerContainer(new IComponentSerializer[]
                { new FactionMemberSerializer(_factionDatabase) }); 

        public void OnCreate(GameObject instance, bool deserialized)
        {
            if (instance.TryGetComponent(out IIdentifiable identifiable))
            {
                (_registrySource ??= GetComponent<IHaveSceneRegistry>()).SceneRegistry.Register(instance);
            }

            if (instance.TryGetComponent(out IRequireComponentSerializerContainer componentRegistry))
            {
                componentRegistry.Container = Container;
            }
        }
    }
}