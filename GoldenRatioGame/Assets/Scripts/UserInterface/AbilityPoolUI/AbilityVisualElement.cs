using IM.Abilities;
using IM.Items;
using IM.Visuals;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityVisualElement : VisualElement
    {
        private readonly MarqueeContainerBase _nameLabel;
        private readonly Image _iconElement;
        
        public IAbilityReadOnly Ability { get; set; }
        
        public AbilityVisualElement()
        {
            AddToClassList(AbilityClassLists.Ability);

            _iconElement = new Image();
            _iconElement.scaleMode = ScaleMode.ScaleToFit;
            
            _nameLabel = new FadingMarqueeContainer()
            {
                DurationSec = 2f,
                FadeDurationSec = 0.5f,
            };
            _nameLabel.AddToClassList(AbilityClassLists.AbilityName);

            Add(_nameLabel);
        }

        public void SetAbility(IAbilityReadOnly ability)
        {
            _nameLabel.Text = ability.Name;

            if (ability is IHaveIcon { Icon: not null } icon)
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