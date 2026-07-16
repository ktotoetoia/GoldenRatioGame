using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class ItemStatsInfoElement : ItemInfoElement
    {
        private const string AugmentContainerClassName = "item-stats-info__augment-container";
        private const string StatsContainerClassName   = "item-stats-info__stats-container";
        private const string RightContainerClassName = "item-stats-info__right-container";

        protected virtual string AugmentContainerClass => AugmentContainerClassName;
        protected virtual string StatsContainerClass => StatsContainerClassName;

        protected VisualElement AugmentContainer { get; }

        private readonly List<IStatPreviewer> _statPreviewers;
        private readonly IStatPreviewer _augmentPreviewer;
        private readonly Dictionary<IStatPreviewer, VisualElement> _statElements = new();

        private VisualElement _statsContainer;
        private VisualElement _augmentElement;
        private VisualElement _rightContainer;

        public ItemStatsInfoElement() : this(Enumerable.Empty<IStatPreviewer>(), null) { }

        public ItemStatsInfoElement(IEnumerable<IStatPreviewer> statPreviewers, IStatPreviewer augmentPreviewer)
        {
            _statPreviewers = statPreviewers?.ToList() ?? new List<IStatPreviewer>();
            _augmentPreviewer = augmentPreviewer;

            AugmentContainer = new VisualElement();
            AugmentContainer.AddToClassList(AugmentContainerClass);
            
            _rightContainer = new VisualElement();
            
            TopRow.Remove(ActionContainer);
            TopRow.Add(_rightContainer);
            
            _rightContainer.AddToClassList(RightContainerClassName);
            
            _rightContainer.Add(AugmentContainer);
            _rightContainer.Add(ActionContainer);
        }

        public override void SetItem(object item)
        {
            base.SetItem(item);

            RebuildStats(item);
            RebuildAugments(item);
        }

        public void UpdatePreviews()
        {
            if (Item == null) return;

            foreach (var (previewer, element) in _statElements)
                previewer.UpdatePreview(element, Item);

            _augmentPreviewer?.UpdatePreview(_augmentElement,Item);
        }

        private void RebuildStats(object item)
        {
            _statsContainer ??= CreateStatsContainer();

            _statsContainer.Clear();
            _statElements.Clear();

            foreach (IStatPreviewer statPreviewer in _statPreviewers)
            {
                VisualElement statElement = statPreviewer.GetPreview(item);

                if (statElement == null) continue;

                _statsContainer.Add(statElement);
                _statElements[statPreviewer] = statElement;
            }

            SetAdditionalInfo(_statsContainer);
        }

        private VisualElement CreateStatsContainer()
        {
            var container = new VisualElement();
            container.AddToClassList(StatsContainerClass);
            return container;
        }

        private void RebuildAugments(object item)
        {
            AugmentContainer.Clear();
            _augmentElement = null;
            
            if (_augmentPreviewer == null || item is not MonoBehaviour mb || !mb.TryGetComponent(out IAugmentSource aug)) return;

            _augmentElement = _augmentPreviewer.GetPreview(item);
            
            AugmentContainer.Add(_augmentElement);
        }
    }
}