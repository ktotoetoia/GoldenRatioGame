using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class WeaponContainerVisualElement : VisualElement
    {
        private readonly ItemVisualElement _itemVisualElement = new();
        private IWeaponContainerReadOnly _container;

        public WeaponContainerVisualElement()
        {
            Add(_itemVisualElement);
        }

        public void SetContainer(IWeaponContainerReadOnly container)
        {
            _container = container;
            container.PreferredWeaponChanged += UpdateItem;
        }

        private void UpdateItem(object obj)
        {
            _itemVisualElement.SetItem(obj);
        }
    }
}