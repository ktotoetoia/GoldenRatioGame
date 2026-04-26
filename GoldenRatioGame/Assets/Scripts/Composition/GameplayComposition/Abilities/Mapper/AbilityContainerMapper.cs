using System.Collections.Generic;
using IM.Abilities;
using IM.WeaponSystem;

namespace IM.Modules
{
    public sealed class AbilityContainerMapper
    {
        public IAbilityContainer Wrap(IAbilityContainer unwrapped)
        {
            switch (unwrapped)
            {
                case null:
                    return null;
                case AbilityContainerWrapped or WeaponContainerWrapped:
                    return unwrapped;
                case IWeaponContainer weaponContainer:
                    return new WeaponContainerWrapped(weaponContainer);
                default:
                    return new AbilityContainerWrapped(unwrapped);
            }
        } 

        public IAbilityContainer UnWrap(IAbilityContainer wrapped)
        {
            switch (wrapped)
            {
                case null:
                    return null;
                case WeaponContainerWrapped weaponWrapper:
                    weaponWrapper.BaseContainer.Weapon = weaponWrapper.Weapon;
                    return weaponWrapper.BaseContainer;
                case AbilityContainerWrapped standardWrapper:
                    return standardWrapper.Target;
                default:
                    return wrapped;
            }
        }
        
        public IAbilityContainer FindWrapped(
            IEnumerable<IAbilityContainer> collection, 
            IAbilityContainer unwrappedTarget)
        {
            if (collection == null || unwrappedTarget == null) return null;

            foreach (IAbilityContainer item in collection)
            {
                if (item is AbilityContainerWrapped standard && standard.Target == unwrappedTarget || item is WeaponContainerWrapped weapon && weapon.BaseContainer == unwrappedTarget || item == unwrappedTarget)
                {
                    return item;
                }
            }

            return null;
        }
    }
}