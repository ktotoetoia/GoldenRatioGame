using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IAnimatedModuleVisualObject : IModuleVisualObject
    {
        bool IsAnimating { get; set; }
        IEnumerable<IAnimationChange> AnimationChanges { get; set; }
    }
}