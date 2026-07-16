using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class TooltipInfoElement : InfoElementBase
    {
        private const string Root               = "tooltip-info";
        private const string ContentClassName    = "tooltip-info__content";
        private const string TopRowClassName     = "tooltip-info__top-row";
        private const string IconContainerName   = "tooltip-info__icon-container";
        private const string IconClassName       = "tooltip-info__icon";
        private const string IconPlaceholderName = "tooltip-info__icon--placeholder";
        private const string TextColClassName    = "tooltip-info__text-column";
        private const string NameClassName       = "tooltip-info__name";
        private const string ShortDescClassName  = "tooltip-info__short-desc";
        private const string DividerClassName    = "tooltip-info__divider";
        private const string DescClassName       = "tooltip-info__description";
        private const string ActionClassName     = "tooltip-info__action";
        private const string AdditionalInfoName  = "tooltip-info__additional-info";
        private const string AdditionalInfoContentClassName = "tooltip-info__additional-info-content";
        private const string ActionContentClassName    = "tooltip-info__action-content";

        protected override string RootClass => Root;
        protected override string ContentClass => ContentClassName;
        protected override string IconContainerClass => IconContainerName;
        protected override string IconClass => IconClassName;
        protected override string IconPlaceholderClass => IconPlaceholderName;
        protected override string NameClass => NameClassName;
        protected override string ShortDescriptionClass => ShortDescClassName;
        protected override string DescriptionClass => DescClassName;
        protected override string ActionClass => ActionClassName;
        protected override string AdditionalInfoClass => AdditionalInfoName;
        protected override string AdditionalInfoContentClass => AdditionalInfoContentClassName;
        protected override string ActionContentClass => ActionContentClassName;
        
        private VisualElement _topRow;
        private VisualElement _divider;

        public bool ShowDivider { get; set; } = true;
        public bool ShowTopRow { get; set; } = true;

        public TooltipInfoElement()
        {
            BuildLayout();
            SetVisible(this, false);
        }

        protected override void BuildLayout()
        {
            _topRow = new VisualElement();
            _topRow.AddToClassList(TopRowClassName);

            var textColumn = new VisualElement();
            textColumn.AddToClassList(TextColClassName);

            textColumn.Add(NameLabel);
            textColumn.Add(ShortDescriptionLabel);

            _topRow.Add(IconContainer);
            _topRow.Add(textColumn);
            _topRow.Add(ActionContainer);

            ContentContainer.Add(_topRow);

            _divider = new VisualElement();
            _divider.AddToClassList(DividerClassName);
            ContentContainer.Add(_divider);

            ContentContainer.Add(DescriptionLabel);

            ContentContainer.Add(AdditionalInfoContainer);
        }

        public void Bind(ITooltipInfo info)
        {
            if (info == null)
            {
                SetVisible(this, false);
                return;
            }

            SetVisible(this, true);
            ApplyIcon(info.Icon);
            ApplyName(info.Name);
            ApplyShortDescription(info.ShortDescription);
            ApplyDescription(info.Description);

            bool hasIcon = ShowIcon && info.Icon != null;
            bool hasName = ShowName && !string.IsNullOrWhiteSpace(info.Name);
            bool hasShortDescription = ShowShortDescription && !string.IsNullOrWhiteSpace(info.ShortDescription);
            bool hasDescription = ShowDescription && !string.IsNullOrWhiteSpace(info.Description);
            bool hasTopRowContent = hasIcon || hasName || hasShortDescription;
            bool showTopRow = ShowTopRow && hasTopRowContent;

            SetVisible(_topRow, showTopRow);

            SetVisible(_divider, ShowDivider && hasDescription && showTopRow);
        }

        public void Unbind()
        {
            AdditionalInfoContainer.Clear();
            ActionContainer.Clear();
            SetVisible(this, false);
        } 
    }
}