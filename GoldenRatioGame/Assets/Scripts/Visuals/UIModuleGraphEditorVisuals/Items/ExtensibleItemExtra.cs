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

        public IAbilityContainer AbilityContainer { get; private set; }

        public ExtensibleItemExtra(IExtensibleItem item, Action<IWeaponContainer> onClear, AbilityPoolEditingService abilityPoolEditingService)
        {
            _item = item;
            _onClear = onClear;
            _abilityPoolEditingService = abilityPoolEditingService;

            _itemVisualElement = new ItemVisualElement();
            Add(_itemVisualElement);

            Update();
        }

        public void Update()
        {
            _itemVisualElement.SetItem(null);
            AbilityContainer = null;

            if (_item.Extensions.TryGet(out IWeaponExtension weaponExtension))
            {
                AbilityContainer = _abilityPoolEditingService.GetWrapped(weaponExtension);
                _itemVisualElement.SetItem(AbilityContainer.Ability);
                _itemVisualElement.RegisterAction(() => _onClear(AbilityContainer as IWeaponContainer));
                return;
            }

            if (_item.Extensions.TryGet(out IAbilityExtension abilityExtension))
            {
                AbilityContainer = _abilityPoolEditingService.GetWrapped(abilityExtension);
                _itemVisualElement.SetItem(AbilityContainer.Ability);
            }
        }
    }
}
