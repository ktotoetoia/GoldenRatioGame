using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    /// <summary>
    /// Displays <see cref="ITooltipInfo"/> data as a compact card.
    /// All fields are optional — missing ones collapse cleanly out of the layout.
    /// </summary>
    [UxmlElement]
    public partial class TooltipInfoElement : VisualElement
    {
        private const string RootClass            = "tooltip-info";
        private const string TopRowClass          = "tooltip-info__top-row";
        private const string IconClass            = "tooltip-info__icon";
        private const string IconPlaceholderClass = "tooltip-info__icon--placeholder";
        private const string TextColClass         = "tooltip-info__text-column";
        private const string NameClass            = "tooltip-info__name";
        private const string ShortDescClass       = "tooltip-info__short-desc";
        private const string DividerClass         = "tooltip-info__divider";
        private const string DescClass            = "tooltip-info__description";

        private readonly VisualElement _topRow;
        private readonly VisualElement _icon;
        private readonly Label _nameLabel;
        private readonly Label _shortDescLabel;
        private readonly VisualElement _divider;
        private readonly Label _descLabel;

        public bool ShowIcon { get; set; } = true;
        public bool ShowName { get; set; } = true;
        public bool ShowShortDescription { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public bool ShowDivider { get; set; } = true;
        public bool ShowTopRow { get; set; } = true;

        public TooltipInfoElement()
        {
            AddToClassList(RootClass);

            _topRow = new VisualElement();
            _topRow.AddToClassList(TopRowClass);

            _icon = new VisualElement();
            _icon.AddToClassList(IconClass);

            var textColumn = new VisualElement();
            textColumn.AddToClassList(TextColClass);

            _nameLabel = new Label();
            _nameLabel.AddToClassList(NameClass);

            _shortDescLabel = new Label();
            _shortDescLabel.AddToClassList(ShortDescClass);

            textColumn.Add(_nameLabel);
            textColumn.Add(_shortDescLabel);

            _topRow.Add(_icon);
            _topRow.Add(textColumn);

            Add(_topRow);

            _divider = new VisualElement();
            _divider.AddToClassList(DividerClass);
            Add(_divider);

            _descLabel = new Label();
            _descLabel.AddToClassList(DescClass);
            Add(_descLabel);

            SetVisible(this, false);
        }

        public void Bind(ITooltipInfo info)
        {
            if (info == null)
            {
                SetVisible(this, false);
                return;
            }

            SetVisible(this, true);

            bool hasIcon = ShowIcon && info.Icon;
            bool hasName = ShowName && !string.IsNullOrWhiteSpace(info.Name);
            bool hasShortDesc = ShowShortDescription && !string.IsNullOrWhiteSpace(info.ShortDescription);
            bool hasDescription = ShowDescription && !string.IsNullOrWhiteSpace(info.Description);

            _icon.style.backgroundImage = hasIcon
                ? new StyleBackground(info.Icon)
                : StyleKeyword.None;

            _icon.EnableInClassList(IconPlaceholderClass, !hasIcon);
            SetVisible(_icon, ShowIcon);

            SetLabel(_nameLabel, hasName ? info.Name : null);
            SetLabel(_shortDescLabel, hasShortDesc ? info.ShortDescription : null);
            SetLabel(_descLabel, hasDescription ? info.Description : null);

            bool hasTopRowContent = hasIcon || hasName || hasShortDesc;
            SetVisible(_topRow, ShowTopRow && hasTopRowContent);

            SetVisible(_divider,
                ShowDivider &&
                hasDescription &&
                ShowTopRow &&
                hasTopRowContent);
        }

        public void Unbind()
        {
            SetVisible(this, false);
        }

        private static void SetVisible(VisualElement element, bool visible)
        {
            element.style.display = visible
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        private static void SetLabel(Label label, string text)
        {
            bool hasText = !string.IsNullOrWhiteSpace(text);

            label.text = hasText ? text : string.Empty;
            SetVisible(label, hasText);
        }
    }
}