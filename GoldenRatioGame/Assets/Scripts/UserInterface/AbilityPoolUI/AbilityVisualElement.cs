using System.Collections.Generic;
using IM.Abilities;
using IM.Items;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityVisualElement : VisualElement
    {
        private readonly Label _nameLabel;
        private readonly Label _descriptionLabel;
        private readonly VisualElement _iconElement;

        public AbilityVisualElement()
        {
            AddToClassList(AbilityClassLists.Ability);

            _iconElement = new VisualElement();
            _iconElement.AddToClassList(AbilityClassLists.AbilityIcon);

            _nameLabel = new Label();
            _nameLabel.AddToClassList(AbilityClassLists.AbilityName);

            _descriptionLabel = new Label();
            _descriptionLabel.AddToClassList(AbilityClassLists.AbilityDescription);

            Add(_iconElement);
            Add(_nameLabel);
            Add(_descriptionLabel);
        }

        public void SetAbility(IAbilityReadOnly ability)
        {
            _nameLabel.text = ability.Name;
            _descriptionLabel.text = ability.Description;

            if (ability is IHaveIcon icon)
                _iconElement.style.backgroundImage = new StyleBackground(icon.Icon.Sprite);
        }
    }
}