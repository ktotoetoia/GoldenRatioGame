using System;
using System.Collections.Generic;
using IM.Augments;
using IM.LifeCycle;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class AugmentScrollView : ScrollView
    {
        private readonly Dictionary<IAugment, ItemInfoElement> _iconElements = new();
        private readonly CollectionDiffer<IAugment> _differ;

        public List<StyleSheet> IconStyleSheets { get; } = new(); 

        public AugmentScrollView() : base(ScrollViewMode.Vertical)
        {
            contentContainer.style.flexDirection = FlexDirection.Row;
            contentContainer.style.flexWrap = Wrap.Wrap;
            contentContainer.style.paddingTop = 4;
            contentContainer.style.paddingBottom = 4;
            contentContainer.style.paddingLeft = 4;
            contentContainer.style.paddingRight = 4;

            _differ = new CollectionDiffer<IAugment>(OnAugmentAdded, OnAugmentRemoved);
        }

        public void SetAugments(IEnumerable<IAugment> augments)
        {
            _differ.Update(augments ?? Array.Empty<IAugment>());
        }

        private void OnAugmentAdded(IAugment augment)
        {
            var newIcon =  new ItemInfoElement
            {
                ShowName = false,
                ShowShortDescription = false,
                ShowDescription = false
            };
            
            newIcon.SetItem(augment);

            foreach (StyleSheet iconStyleSheet in IconStyleSheets)
            {
                newIcon.styleSheets.Add(iconStyleSheet);
            }
            _iconElements[augment] = newIcon;
            Add(newIcon);
        }

        private void OnAugmentRemoved(IAugment augment)
        {
            if (_iconElements.TryGetValue(augment, out ItemInfoElement iconToRemove))
            {
                Remove(iconToRemove);
                _iconElements.Remove(augment);
            }
        }
    }
}