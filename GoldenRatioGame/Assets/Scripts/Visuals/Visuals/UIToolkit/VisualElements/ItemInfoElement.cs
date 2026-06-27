using IM.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class ItemInfoElement : InfoElementBase, ITooltipInfo
    {
        private const string Root = "item-info";
        private const string IconClassName = "item-info__icon";
        private const string IconPlaceholderName = "item-info__icon--placeholder";
        private const string TextColClassName = "item-info__text-column";
        private const string NameClassName = "item-info__name";
        private const string ShortDescClassName = "item-info__short-desc";
        private const string AdditionalInfoName = "item-info__additional-info";
        
        protected override string RootClass => Root;
        protected override string IconClass => IconClassName;
        protected override string IconPlaceholderClass => IconPlaceholderName;
        protected override string NameClass => NameClassName;
        protected override string ShortDescriptionClass => ShortDescClassName;
        protected override string AdditionalInfoClass => AdditionalInfoName;

        public string Name { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public object Item { get; private set; }

        public ItemInfoElement() => BuildLayout();

        protected override void BuildLayout()
        {
            var textColumn = new VisualElement();
            textColumn.AddToClassList(TextColClassName);

            textColumn.Add(NameLabel);
            textColumn.Add(ShortDescriptionLabel);

            Add(IconElement);
            Add(textColumn);
            Add(AdditionalInfoContainer);
        }

        public void SetItem(object item)
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