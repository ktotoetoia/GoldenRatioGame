using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IAnimatedModuleVisualObject : IModuleVisualObject
    {
        public IEnumerable<IAnimationChange> AnimationChanges { get; set; }
    }
}