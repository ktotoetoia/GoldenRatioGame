using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IModuleAnimator
    {
        IEnumerable<IAnimationContext> CurrentAnimationContexts { get; }
        
        bool CanPlay(IAnimationContext context);
        void Play(IAnimationContext context);
        void Stop(IAnimationContext context);
    }
}