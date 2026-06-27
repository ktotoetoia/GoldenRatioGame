using UnityEngine;

namespace IM.Visuals
{
    public interface ITooltipInfo
    {
        string Name { get; }
        string ShortDescription { get; }
        string Description { get; }
        Sprite Icon { get; }
        object Item { get; }
    }
}