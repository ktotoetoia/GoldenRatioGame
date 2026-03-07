using IM.Abilities;
using IM.Items;
using IM.Visuals;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityVisualElement : VisualElement
    {
        private readonly MarqueeContainer _nameLabel;
        private readonly VisualElement _iconElement;
        
        public IAbilityReadOnly Ability { get; set; }
        
        public AbilityVisualElement()
        {
            AddToClassList(AbilityClassLists.Ability);

            _iconElement = new VisualElement();
            _iconElement.AddToClassList(AbilityClassLists.AbilityIcon);

            _nameLabel = new MarqueeContainer() {DurationMs = 1000};
            _nameLabel.AddToClassList(AbilityClassLists.AbilityName);

            Add(_nameLabel);
        }

        public void SetAbility(IAbilityReadOnly ability)
        {
            _nameLabel.Text = ability.Name;

            if (ability is IHaveIcon { Icon: not null } icon)
            {
                if (_iconElement.parent == null)
                    Insert(0, _iconElement);

                _iconElement.style.backgroundImage = new StyleBackground(icon.Icon.Sprite);
            }
            else
            {
                _iconElement.RemoveFromHierarchy();
            }
        }
    }
}