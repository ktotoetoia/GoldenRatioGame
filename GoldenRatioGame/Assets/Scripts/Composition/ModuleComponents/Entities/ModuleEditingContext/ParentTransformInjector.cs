using IM.LifeCycle;
using UnityEngine;

namespace IM
{
    public class ParentTransformInjector : MonoBehaviour, IGameObjectFactoryObserver
    {
        [SerializeField] private Transform _defaultParentTransform;
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            if(instance.TryGetComponent(out IRequireDefaultParentTransform require))
            {
                require.DefaultParentTransform = _defaultParentTransform;
            }
        }
    }
}