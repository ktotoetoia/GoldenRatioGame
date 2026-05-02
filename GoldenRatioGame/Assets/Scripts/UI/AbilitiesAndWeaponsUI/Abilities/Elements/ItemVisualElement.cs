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
        private readonly Button _actionButton;
        private System.Action _currentAction;
        
        [UxmlAttribute]
        public bool ShowButton
        {
            get => _actionButton.style.display == DisplayStyle.Flex;
            set => _actionButton.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public ItemVisualElement()
        {
            AddToClassList(ItemClassLists.Item);

            _iconElement = new Image
            {
                scaleMode = ScaleMode.ScaleToFit
            };

            _nameLabel = new FadingMarqueeContainer()
            {
                DurationSec = 2f,
                FadeDurationSec = 0.5f,
            };

            _actionButton = new Button();

            _nameLabel.AddToClassList(ItemClassLists.ItemName);
            _iconElement.AddToClassList(ItemClassLists.ItemIcon);
            _actionButton.AddToClassList(ItemClassLists.ItemButton);

            ShowButton = false;

            Add(_actionButton);
            Add(_nameLabel);
        }
        
        public void RegisterAction(string label, System.Action callback)
        {
            if (_currentAction != null) _actionButton.clicked -= _currentAction;

            _currentAction = callback;

            _actionButton.text = label;
            _actionButton.clicked += _currentAction;
            ShowButton = true;
        }
        
        public void UnregisterActions()
        {
            if (_currentAction != null)
            {
                _actionButton.clicked -= _currentAction;
                _currentAction = null;
            }

            ShowButton = false;
        }

        public void SetItem(object item)
        {
            if (item is IHaveName named) _nameLabel.Text = named.Name;

            if (item is IHaveIcon { Icon: not null } icon)
            {
                _nameLabel.RemoveFromHierarchy();

                if (_iconElement.parent == null) Add(_iconElement);

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