namespace IM.Visuals
{
    public interface IModuleSubAnimator
    {
        IAnimationContext CurrentAnimationContext { get; }

        bool CanPlay(IAnimationContext context);
        void Play(IAnimationContext context);
        void Stop();
    }
}