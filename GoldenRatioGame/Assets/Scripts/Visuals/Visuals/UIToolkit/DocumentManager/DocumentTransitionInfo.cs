using UnityEngine.UIElements;

namespace IM.UI
{
    public readonly struct DocumentTransitionInfo
    {
        public readonly UIDocument FromDocument;
        public readonly UIDocument ToDocument;
        public readonly bool IsOverlay;
        public readonly int DefaultSortingOrder;

        public DocumentTransitionInfo(UIDocument fromDocument, UIDocument toDocument, bool isOverlay = false, int defaultSortingOrder = 0)
        {
            FromDocument = fromDocument;
            ToDocument = toDocument;
            IsOverlay = isOverlay;
            DefaultSortingOrder = defaultSortingOrder;
        }
    }
}