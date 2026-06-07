using IM.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class ItemDisplayContainer : VisualElement
    {
        private readonly VisualElement _icon;
        private readonly MarqueeContainerBase _nameMarquee;
        private readonly MarqueeContainerBase _descriptionMarquee;
        private readonly VisualElement _extraSlot;

        public ItemDisplayContainer()
        {
            AddToClassList("itemDisplayContainer");

            _icon = new VisualElement();
            _icon.AddToClassList("itemIcon");
            Add(_icon);

            var infoColumn = new VisualElement();
            infoColumn.AddToClassList("itemInfoColumn");

            _nameMarquee = new MarqueeContainerBase();
            _nameMarquee.AddToClassList("itemName");
            _nameMarquee.style.flexGrow = 0;
            infoColumn.Add(_nameMarquee);

            _descriptionMarquee = new MarqueeContainerBase();
            _descriptionMarquee.AddToClassList("itemDescription");
            _descriptionMarquee.style.flexGrow = 0;
            infoColumn.Add(_descriptionMarquee);

            Add(infoColumn);

            _extraSlot = new VisualElement();
            _extraSlot.AddToClassList("itemExtraSlot");
            Add(_extraSlot);
        }

        public void SetItem(object item)
        {
            if (item == null)
            {
                _nameMarquee.Text = string.Empty;
                _descriptionMarquee.Text = string.Empty;
                _icon.style.backgroundImage = StyleKeyword.None;
                return;
            }

            _nameMarquee.Text = item is IHaveName named ? named.Name : string.Empty;
            _descriptionMarquee.Text = item is IHaveDescription described ? described.Description : string.Empty;
            _icon.style.backgroundImage = item is IHaveIcon { Icon: not null } iconed
                ? Background.FromSprite(iconed.Icon.Sprite)
                : StyleKeyword.None;
        }

        public VisualElement ExtraElement { get; private set; }

        public void SetExtraElement(VisualElement element)
        {
            ExtraElement = element;
            _extraSlot.Clear();
            if (element != null)
                _extraSlot.Add(element);
        }
    }
}
