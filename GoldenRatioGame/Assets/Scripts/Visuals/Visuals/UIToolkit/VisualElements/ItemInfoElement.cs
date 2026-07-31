using IM.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class ItemInfoElement : InfoElementBase, ITooltipInfo
    {
        private const string Root                = "item-info";
        private const string ContentClassName    = "item-info__content";
        private const string TopRowClassName     = "item-info__top-row";
        private const string IconContainerName   = "item-info__icon-container";
        private const string IconClassName       = "item-info__icon";
        private const string IconPlaceholderName = "item-info__icon--placeholder";
        private const string TextColClassName    = "item-info__text-column";
        private const string NameClassName       = "item-info__name";
        private const string ShortDescClassName  = "item-info__short-desc";
        private const string ActionClassName     = "item-info__action";
        private const string AdditionalInfoName  = "item-info__additional-info";
        private const string AdditionalInfoContentClassName = "item-info__additional-info-content";
        private const string ActionContentClassName = "item-info__action-content";

        protected override string RootClass => Root;
        protected override string ContentClass => ContentClassName;
        protected override string IconContainerClass => IconContainerName;
        protected override string IconClass => IconClassName;
        protected override string IconPlaceholderClass => IconPlaceholderName;
        protected override string NameClass => NameClassName;
        protected override string ShortDescriptionClass => ShortDescClassName;
        protected override string ActionClass => ActionClassName;
        protected override string AdditionalInfoClass => AdditionalInfoName;
        protected override string AdditionalInfoContentClass => AdditionalInfoContentClassName;
        protected override string ActionContentClass => ActionContentClassName;

        protected VisualElement TopRow { get; private set; }

        public string Name { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public object Item { get; private set; }
        public bool TooltipDisabled { get; set; }

        public ItemInfoElement() => BuildLayout();

        protected override void BuildLayout()
        {
            TopRow = new VisualElement();
            TopRow.AddToClassList(TopRowClassName);

            var textColumn = new VisualElement();
            textColumn.AddToClassList(TextColClassName);

            textColumn.Add(NameLabel);
            textColumn.Add(ShortDescriptionLabel);

            TopRow.Add(IconContainer);
            TopRow.Add(textColumn);
            TopRow.Add(ActionContainer);

            ContentContainer.Add(TopRow);

            ContentContainer.Add(AdditionalInfoContainer);
        }

        public virtual void SetItem(object item)
        {
            Item = item;
            Name = item is IHaveName named ? named.Name : null;
            Icon = item is IHaveIcon { Icon: not null } hasIcon ? hasIcon.Icon.Sprite : null;

            if (item is IHaveDescription described)
            {
                ShortDescription = described.ShortDescription;
                Description = described.Description;
            }
            else
            {
                ShortDescription = null;
                Description = null;
            }

            ApplyIcon(Icon);
            ApplyName(Name);
            ApplyShortDescription(ShortDescription);
            schedule.Execute(MarkDirtyRepaint);
        }
    }
}