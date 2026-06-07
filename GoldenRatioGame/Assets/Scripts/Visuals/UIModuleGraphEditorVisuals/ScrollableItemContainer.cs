using System.Collections.Generic;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class ScrollableItemContainer : VisualElement
    {
        private readonly ScrollView _scrollView;

        [UxmlAttribute]
        public ScrollViewMode ScrollMode
        {
            get => _scrollView.mode;
            set => _scrollView.mode = value;
        }

        public ScrollableItemContainer()
        {
            AddToClassList("scrollableItemContainer");

            _scrollView = new ScrollView(ScrollViewMode.Vertical);
            _scrollView.AddToClassList("scrollableItemContainerView");
            Add(_scrollView);
        }

        public void SetItems(IEnumerable<VisualElement> items)
        {
            _scrollView.Clear();
            foreach (var item in items)
                _scrollView.Add(item);
        }

        public void AddItem(VisualElement item) => _scrollView.Add(item);

        public void RemoveItem(VisualElement item) => _scrollView.Remove(item);

        public void ClearItems() => _scrollView.Clear();
    }
}
