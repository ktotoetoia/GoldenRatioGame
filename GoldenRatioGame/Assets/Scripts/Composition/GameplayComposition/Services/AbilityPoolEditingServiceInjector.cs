using System;
using System.Linq;
using IM.Abilities;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolEditingServiceInjector : MonoBehaviour, IModuleEditingContextObserver, IFactory<object>,ICapabilitySnapshot
    {
        public Type CapabilityType => typeof(ContainerAbilityPool);

        public object Snapshot(object capability)
        {
            var pool = (ContainerAbilityPool)capability;
            return new ContainerAbilityPool(pool.AbilityContainers.ToList());
        }

        public object Create()
        {
            return new ContainerAbilityPool();
        }
        
        public void OnContextCreated(IModuleEditingContext context)
        {
            if(!context.Capabilities.TryGet(out IContainerAbilityPool pool) ||
               context.GraphEditing is not IGraphEditingEvents<IExtensibleItem> events) return;

            context.AddService(new AbilityPoolEditingService(events, pool));
        }
    }
}