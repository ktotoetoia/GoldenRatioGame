using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationChange
    {
        AnimationChangeType AnimationChangeType { get; }
        public string ParameterName { get; }
        void ApplyToAnimator(Animator animator);
    }
}