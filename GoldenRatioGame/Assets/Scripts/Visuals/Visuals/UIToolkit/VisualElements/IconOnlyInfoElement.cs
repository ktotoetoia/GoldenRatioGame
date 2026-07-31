using IM.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class IconOnlyInfoElement : VisualElement, ITooltipInfo
    {
        private const string RootClassName = "icon-only-info";

        public string Name { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public object Item { get; private set; }
        public bool TooltipDisabled { get; set; }

        public IconOnlyInfoElement()
        {
            AddToClassList(RootClassName);
            style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            style.flexGrow = 1;
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

            ApplyIconAsBackground(Icon);
            schedule.Execute(MarkDirtyRepaint);
        }

        protected virtual void ApplyIconAsBackground(Sprite sprite)
        {
            if (sprite != null)
            {
                style.backgroundImage = new StyleBackground(sprite);
                style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            }
            else
            {
                style.backgroundImage = StyleKeyword.Null;
            }
        }
    }
}