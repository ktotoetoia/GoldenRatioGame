using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class AugmentPreviewElement : VisualElement, ITooltipInfo
    {
        private const string RootClass             = "augment-preview";
        private const string IconClass             = "augment-preview__icon";
        private const string LabelClass            = "augment-preview__label";
        private const string ShortDescriptionClass = "augment-preview__short-desc";
        private const string FinishedClass         = "augment-preview--finished";
        private const string NotStartedClass       = "augment-preview--not-started";

        private readonly VisualElement _icon;
        private readonly Label _label;
        private readonly Label _shortDescriptionLabel;

        public AugmentInfo? AugmentInfo { get; private set; }
        public IAugment Augment { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsCurrent { get; private set; }
        public bool IsNotStarted { get; private set; }
        public bool ShowShortDescription { get; set; } = false;

        public Sprite Icon => Augment?.Icon?.Sprite;
        public string Name => Augment?.Name;
        public string ShortDescription => Augment?.ShortDescription;
        public string Description => Augment?.Description;
        public object Item =>  Augment;
        public bool TooltipDisabled { get; set; }

        public AugmentPreviewElement()
        {
            AddToClassList(RootClass);

            _icon = new VisualElement();
            _icon.AddToClassList(IconClass);
            Add(_icon);

            _label = new Label();
            _label.AddToClassList(LabelClass);
            _icon.Add(_label);

            _shortDescriptionLabel = new Label();
            _shortDescriptionLabel.AddToClassList(ShortDescriptionClass);
            Add(_shortDescriptionLabel);
            if(!ShowShortDescription) _shortDescriptionLabel.style.display = DisplayStyle.None;
        }

        public void SetAugment(AugmentInfo augmentInfo, IAugment augment, float currentProgress, bool isCurrent = false)
        {
            AugmentInfo = augmentInfo;
            Augment = augment;
            
            Sprite icon = augment?.Icon?.Sprite;
            _icon.style.backgroundImage = icon ? new StyleBackground(icon) : StyleKeyword.None;

            float required = Mathf.Max(0f, augmentInfo.RequiredProgress);
            float remaining = Mathf.Max(0f, required - currentProgress);

            IsFinished = required <= 0f || currentProgress >= required;
            IsCurrent = isCurrent && !IsFinished;
            IsNotStarted = !IsFinished && !IsCurrent && currentProgress <= 0f;

            EnableInClassList(FinishedClass, IsFinished);
            EnableInClassList(NotStartedClass, IsNotStarted);

            _label.text ="<sprite name=A" + (IsFinished ? "Check" :remaining.ToString("0") )+">";

            if (IsCurrent) _label.text += "<sprite name=ANext>";

            bool hasShortDescription = !string.IsNullOrWhiteSpace(augment?.ShortDescription);
            _shortDescriptionLabel.text = hasShortDescription ? augment.ShortDescription : string.Empty;
            _shortDescriptionLabel.style.display = hasShortDescription && ShowShortDescription? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void Clear()
        {
            AugmentInfo = null;
            Augment = null;
            IsFinished = false;
            IsCurrent = false;
            IsNotStarted = false;

            _icon.style.backgroundImage = StyleKeyword.None;
            _label.text = string.Empty;

            _shortDescriptionLabel.text = string.Empty;
            _shortDescriptionLabel.style.display = DisplayStyle.None;

            RemoveFromClassList(FinishedClass);
            RemoveFromClassList(NotStartedClass);
        }
    }
}