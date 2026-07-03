using System;
using IM.Abilities;
using IM.Modules;
using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ExtensibleItemExtra : VisualElement
    {
        private readonly Action<IWeaponContainer> _onClear;
        private readonly AbilityPoolEditingService _abilityPoolEditingService;
        private readonly ItemVisualElement _itemVisualElement;
        private readonly IExtensibleItem _item;
        private readonly Action _clearAction;

        public IAbilityContainer AbilityContainer { get; private set; }

        private object _lastAbility;

        public ExtensibleItemExtra(IExtensibleItem item, Action<IWeaponContainer> onClear, AbilityPoolEditingService abilityPoolEditingService)
        {
            _item = item;
            _onClear = onClear;
            _abilityPoolEditingService = abilityPoolEditingService;
            _clearAction = () => _onClear(AbilityContainer as IWeaponContainer);

            _itemVisualElement = new ItemVisualElement();
            Add(_itemVisualElement);

            Update();
        }

        public void Update()
        {
            IAbilityContainer resolvedContainer = null;
            bool isWeapon = false;

            if (_item.Extensions.TryGet(out IWeaponExtension weaponExtension))
            {
                resolvedContainer = _abilityPoolEditingService.GetWrapped(weaponExtension);
                isWeapon = true;
            }
            else if (_item.Extensions.TryGet(out IAbilityExtension abilityExtension))
            {
                resolvedContainer = _abilityPoolEditingService.GetWrapped(abilityExtension);
            }

            object resolvedAbility = resolvedContainer?.Ability;

            if (ReferenceEquals(resolvedContainer, AbilityContainer) && ReferenceEquals(resolvedAbility, _lastAbility))
                return;

            AbilityContainer = resolvedContainer;
            _lastAbility = resolvedAbility;

            _itemVisualElement.SetItem(resolvedAbility);

            if (isWeapon)
                _itemVisualElement.RegisterAction(_clearAction);
        }
    }
}