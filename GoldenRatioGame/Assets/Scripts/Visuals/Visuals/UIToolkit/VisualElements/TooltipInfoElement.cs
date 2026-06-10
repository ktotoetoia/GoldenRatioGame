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
        private readonly Label         _nameLabel;
        private readonly Label         _shortDescLabel;
        private readonly VisualElement _divider;
        private readonly Label         _descLabel;

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

            Show(this, false);
        }

        public void Bind(ITooltipInfo info)
        {
            if (info == null) { Show(this, false); return; }

            Show(this, true);

            bool hasIcon = info.Icon != null;
            _icon.style.backgroundImage = hasIcon
                ? new StyleBackground(info.Icon)
                : StyleKeyword.None;
            _icon.EnableInClassList(IconPlaceholderClass, !hasIcon);

            SetLabel(_nameLabel,     info.Name);
            SetLabel(_shortDescLabel, info.ShortDescription);

            bool hasDesc = !string.IsNullOrEmpty(info.Description);
            Show(_divider, hasDesc);
            SetLabel(_descLabel, info.Description);

            Show(_topRow, hasIcon
                          || !string.IsNullOrEmpty(info.Name)
                          || !string.IsNullOrEmpty(info.ShortDescription));
        }

        public void Unbind() => Show(this, false);

        private static void Show(VisualElement el, bool visible) =>
            el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

        private static void SetLabel(Label label, string text)
        {
            bool hasText = !string.IsNullOrEmpty(text);
            label.text = hasText ? text : string.Empty;
            Show(label, hasText);
        }
    }
}
