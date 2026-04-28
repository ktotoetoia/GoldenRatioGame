using IM.Items;
using IM.Visuals;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ItemVisualElement : VisualElement
    {
        private readonly MarqueeContainerBase _nameLabel;
        private readonly Image _iconElement;
        
        public ItemVisualElement()
        {
            AddToClassList(AbilityClassLists.Ability);

            _iconElement = new Image
            {
                scaleMode = ScaleMode.ScaleToFit
            };

            _nameLabel = new FadingMarqueeContainer()
            {
                DurationSec = 2f,
                FadeDurationSec = 0.5f,
            };
            _nameLabel.AddToClassList(AbilityClassLists.AbilityName);

            Add(_nameLabel);
        }

        public void SetItem(object item)
        {
            if(item is IHaveName named) _nameLabel.Text = named.Name;

            if (item is IHaveIcon { Icon: not null } icon)
            {
                _nameLabel.RemoveFromHierarchy();
                
                if (_iconElement.parent == null)
                    Add(_iconElement);

                _iconElement.sprite = icon.Icon.Sprite;
            }
            else
            {
                Add(_nameLabel);
                _iconElement.RemoveFromHierarchy();
            }
        }
    }
}