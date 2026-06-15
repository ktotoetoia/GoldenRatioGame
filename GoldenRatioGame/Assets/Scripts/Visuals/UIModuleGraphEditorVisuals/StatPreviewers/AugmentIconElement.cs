using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class AugmentIconElement : VisualElement, ITooltipInfo
    {
        private readonly IAugment _augment;

        public string Name => _augment?.Name;
        public string ShortDescription => _augment?.ShortDescription;
        public string Description => _augment?.Description;
        public Sprite Icon => _augment?.Icon.Sprite;

        public AugmentIconElement(IAugment augment)
        {
            _augment = augment;

            style.width = 64;
            style.height = 64;
            style.marginRight = 4;
            style.marginBottom = 4;

            if (augment?.Icon != null) style.backgroundImage = new StyleBackground(augment.Icon.Sprite);
            else style.backgroundColor = new StyleColor(Color.gray);
        }
    }
}