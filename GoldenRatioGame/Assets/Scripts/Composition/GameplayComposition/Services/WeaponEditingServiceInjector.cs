using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponEditingServiceInjector : MonoBehaviour, IModuleEditingContextObserver
    {
        public void OnContextCreated(IModuleEditingContext context)
        {
            if(!context.Capabilities.TryGet(out IContainerAbilityPool containerAbilityPool)) return;

            context.AddService(new WeaponEditingService(containerAbilityPool,context.StorageEditing));
        }
    }
}