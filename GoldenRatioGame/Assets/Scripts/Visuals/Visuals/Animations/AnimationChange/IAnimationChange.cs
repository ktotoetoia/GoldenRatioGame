using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationChange
    {
        AnimationChangeType AnimationChangeType { get; }
        public string PropertyName { get; }
        void ApplyToAnimator(Animator animator);
    }
}