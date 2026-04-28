using IM.Abilities;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponEditingServiceInjector : MonoBehaviour, IModuleEditingContextObserver
    {
        public void OnContextCreated(IModuleEditingContext context)
        {
            if(context.Storage is not ICellFactoryStorage storage || !context.Capabilities.TryGet(out IContainerAbilityPool containerAbilityPool)) return;

            context.AddService(new WeaponEditingService(containerAbilityPool,storage));
        }
    }
}