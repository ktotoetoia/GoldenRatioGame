using UnityEngine.UIElements;

namespace IM.Visuals
{
  [UxmlElement]
    public partial class TooltipInfoElement : InfoElementBase
    {
        private const string Root = "tooltip-info";
        private const string TopRowClassName = "tooltip-info__top-row";
        private const string IconClassName = "tooltip-info__icon";
        private const string IconPlaceholderName = "tooltip-info__icon--placeholder";
        private const string TextColClassName = "tooltip-info__text-column";
        private const string NameClassName = "tooltip-info__name";
        private const string ShortDescClassName = "tooltip-info__short-desc";
        private const string DividerClassName = "tooltip-info__divider";
        private const string DescClassName = "tooltip-info__description";
        private const string AdditionalInfoName = "tooltip-info__additional-info";

        protected override string RootClass => Root;
        protected override string IconClass => IconClassName;
        protected override string IconPlaceholderClass => IconPlaceholderName;
        protected override string NameClass => NameClassName;
        protected override string ShortDescriptionClass => ShortDescClassName;
        protected override string DescriptionClass => DescClassName;
        protected override string AdditionalInfoClass => AdditionalInfoName;

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

            _topRow.Add(IconElement);
            _topRow.Add(textColumn);

            Add(_topRow);

            _divider = new VisualElement();
            _divider.AddToClassList(DividerClassName);
            Add(_divider);

            Add(DescriptionLabel);
            Add(AdditionalInfoContainer);
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
            ClearAdditionalInfo();
            SetVisible(this, false);
        }
    }
}