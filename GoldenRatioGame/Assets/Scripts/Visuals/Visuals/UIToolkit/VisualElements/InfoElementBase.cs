using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public abstract class InfoElementBase : VisualElement
    {
        protected readonly VisualElement ContentContainer;
        protected readonly VisualElement IconContainer;
        protected readonly VisualElement IconElement;
        protected readonly FadingMarqueeContainer NameLabel;
        protected readonly FadingMarqueeContainer ShortDescriptionLabel;
        protected readonly Label DescriptionLabel;
        protected readonly VisualElement ActionContainer;
        protected readonly VisualElement AdditionalInfoContainer;

        public bool ShowIcon { get; set; } = true;
        public bool ShowName { get; set; } = true;
        public bool ShowShortDescription { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public VisualElement Action { get; private set; }
        public VisualElement AdditionalInfo { get; private set; }

        protected InfoElementBase()
        {
            AddToClassList(RootClass);

            ContentContainer = new VisualElement();
            ContentContainer.AddToClassList(ContentClass);
            Add(ContentContainer);

            IconContainer = new VisualElement();
            IconContainer.AddToClassList(IconContainerClass);

            IconElement = new VisualElement();
            IconElement.AddToClassList(IconClass);
            IconContainer.Add(IconElement);

            NameLabel = new FadingMarqueeContainer();
            NameLabel.AddToClassList(NameClass);

            ShortDescriptionLabel = new FadingMarqueeContainer();
            ShortDescriptionLabel.AddToClassList(ShortDescriptionClass);

            DescriptionLabel = new Label();
            DescriptionLabel.AddToClassList(DescriptionClass);

            ActionContainer = new VisualElement();
            ActionContainer.AddToClassList(ActionClass);
            SetVisible(ActionContainer, false);

            AdditionalInfoContainer = new VisualElement();
            AdditionalInfoContainer.AddToClassList(AdditionalInfoClass);
            SetVisible(AdditionalInfoContainer, false);
        }

        protected virtual string RootClass => "info-element";
        protected virtual string ContentClass => "info-element__content";
        protected virtual string IconContainerClass => "info-element__icon-container";
        protected virtual string IconClass => "info-element__icon";
        protected virtual string IconPlaceholderClass => "info-element__icon--placeholder";
        protected virtual string TextColumnClass => "info-element__text-column";
        protected virtual string NameClass => "info-element__name";
        protected virtual string ShortDescriptionClass => "info-element__short-desc";
        protected virtual string DescriptionClass => "info-element__description";
        protected virtual string ActionClass => "info-element__action";
        protected virtual string AdditionalInfoClass => "info-element__additional-info";

        protected abstract void BuildLayout();

        protected void ApplyIcon(Sprite icon)
        {
            bool hasIcon = ShowIcon && icon;

            IconElement.style.backgroundImage = hasIcon ? new StyleBackground(icon) : StyleKeyword.None;
            IconElement.EnableInClassList(IconPlaceholderClass, !hasIcon);

            SetVisible(IconContainer, ShowIcon);
        }

        protected void ApplyName(string name) => SetLabel(NameLabel, ShowName ? name : null);
        protected void ApplyShortDescription(string description) => SetLabel(ShortDescriptionLabel, ShowShortDescription ? description : null);
        protected void ApplyDescription(string description) => SetLabel(DescriptionLabel, ShowDescription ? description : null);

        public void SetAction(VisualElement action)
        {
            Action = action;

            ActionContainer.Clear();

            if (action != null) ActionContainer.Add(action);

            SetVisible(ActionContainer, action != null);
        }

        public void ClearAction()
        {
            Action = null;

            ActionContainer.Clear();
            SetVisible(ActionContainer, false);
        }

        public void SetAdditionalInfo(VisualElement content)
        {
            AdditionalInfo = content;

            AdditionalInfoContainer.Clear();

            if (content != null) AdditionalInfoContainer.Add(content);

            SetVisible(AdditionalInfoContainer, content != null);
        }

        public void ClearAdditionalInfo()
        {
            AdditionalInfo = null;

            AdditionalInfoContainer.Clear();
            SetVisible(AdditionalInfoContainer, false);
        }

        protected static void SetVisible(VisualElement element, bool visible)
        {
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        protected static void SetLabel(MarqueeContainerBase label, string text)
        {
            bool hasText = !string.IsNullOrWhiteSpace(text);

            SetVisible(label, hasText);
            label.Text = hasText ? text : string.Empty;
        }

        protected static void SetLabel(Label label, string text)
        {
            bool hasText = !string.IsNullOrWhiteSpace(text);

            SetVisible(label, hasText);
            label.text = hasText ? text : string.Empty;
        }
    }
}