using System;
using IM.LifeCycle;
using IM.SaveSystem;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponExtensionSerializer : ComponentSerializer<WeaponExtension>
    {
        public override object CaptureState(WeaponExtension component)
        {
            return (component.Weapon as MonoBehaviour)?.GetComponent<IIdentifiable>()?.Id ?? "";
        }

        public override void RestoreState(WeaponExtension component, object state, Func<string, GameObject> resolveDependency)
        {
            if(state is not string str || string.IsNullOrEmpty(str) || !resolveDependency(str).TryGetComponent(out IWeapon weapon)) return;

            component.Weapon = weapon;
        }
    }
}