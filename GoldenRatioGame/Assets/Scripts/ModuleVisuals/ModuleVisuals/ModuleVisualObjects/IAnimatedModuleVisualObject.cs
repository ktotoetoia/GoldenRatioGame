using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IAnimatedModuleVisualObject : IModuleVisualObject
    {
        bool Animate { get; set; }
        IEnumerable<IAnimationChange> AnimationChanges { get; set; }
    }
}