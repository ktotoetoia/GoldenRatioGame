using UnityEngine;

namespace IM.Visuals
{
    public interface IAnimationChange
    {
        ParameterType ParameterType { get; }
        public string ParameterName { get; }
        void ApplyToAnimator(Animator animator);
    }
}