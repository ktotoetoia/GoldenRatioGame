using System;
using IM.Abilities;
using IM.Modules;
using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ExtensibleItemExtra : VisualElement
    {
        private readonly AbilityPoolEditingService _abilityPoolEditingService;
        private readonly IconOnlyInfoElement _iconVisualElement;
        private readonly IExtensibleItem _item;
        private readonly Action _clearAction;
        private Action _currentAction;

        public IAbilityContainer AbilityContainer { get; private set; }

        private object _lastAbility;

        public ExtensibleItemExtra(IExtensibleItem item, Action<IWeaponContainer> onClear, AbilityPoolEditingService abilityPoolEditingService)
        {
            _item = item;
            _abilityPoolEditingService = abilityPoolEditingService;
            _clearAction = () => onClear(AbilityContainer as IWeaponContainer);
            style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            style.flexGrow = 1;

            _iconVisualElement = new IconOnlyInfoElement();
            Add(_iconVisualElement);

            this.AddManipulator(new Clickable(OnClick));
            Update();
        }
        private void OnClick()
        {
            _currentAction?.Invoke();
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

            _iconVisualElement.SetItem(resolvedAbility);

            _currentAction = isWeapon ? _clearAction : null;
        }
    }
}