using UnityEngine;

namespace IM.Visuals
{
    public class TooltipInfoWrapper : ITooltipInfo
    {
        public string Name { get; }
        public string ShortDescription { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        
        public TooltipInfoWrapper(string name, string shortDescription, string description, Sprite icon)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            Icon = icon;
        }
    }
}