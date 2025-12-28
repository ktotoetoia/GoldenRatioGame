using TDS.Events;
using TDS.Handlers;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEventsExtensionComponent : MonoBehaviour, IEventBus, IModuleExtension
    {
        private readonly EventBus _eventBus = new();
        
        public void Publish<TType>(TType obj)
        {
            _eventBus.Publish(obj);
        }

        public void Subscribe<TType>(IHandler<TType> handler)
        {
            _eventBus.Subscribe(handler);
        }
        
        public void Unsubscribe<TType>(IHandler<TType> handler)
        {
            _eventBus.Unsubscribe(handler);
        }
    }
}