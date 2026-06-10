using System;
using IM.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ItemVisualElement : VisualElement
    {
        private readonly MarqueeContainerBase _nameLabel;
        private readonly Image _iconElement;
        private Action _currentAction;

        public ItemVisualElement()
        {
            AddToClassList(ItemClassLists.Item);

            _iconElement = new Image { scaleMode = ScaleMode.ScaleToFit };
            _iconElement.AddToClassList(ItemClassLists.ItemIcon);

            _nameLabel = new FadingMarqueeContainer
            {
                DurationSec = 2f,
                FadeDurationSec = 0.5f,
            };
            _nameLabel.AddToClassList(ItemClassLists.ItemName);

            Add(_nameLabel);

            this.AddManipulator(new Clickable(OnClick));
        }

        private void OnClick()
        {
            _currentAction?.Invoke();
        }

        public void RegisterAction(Action callback)
        {
            _currentAction = callback;
            _iconElement.AddToClassList(ItemClassLists.ItemIconActive); 
        }

        public void UnregisterAction()
        {
            _currentAction = null;
            _iconElement.RemoveFromClassList(ItemClassLists.ItemIconActive);
        }

        public void SetItem(object item)
        {
            UnregisterAction();

            if (item == null)
            {
                _nameLabel.Text = string.Empty;
                if (_nameLabel.parent == null) Add(_nameLabel);
                _iconElement.RemoveFromHierarchy();
                return;
            }

            if (item is IHaveName named) _nameLabel.Text = named.Name;

            if (item is IHaveIcon { Icon: not null } icon)
            {
                _nameLabel.RemoveFromHierarchy();
                if (_iconElement.parent == null) Add(_iconElement);
                _iconElement.sprite = icon.Icon.Sprite;
            }
            else
            {
                if (_nameLabel.parent == null) Add(_nameLabel);
                _iconElement.RemoveFromHierarchy();
            }
        }
    }
}