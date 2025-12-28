using UnityEngine;

namespace IM.Visuals
{
    public class AnimationModule : VisualModuleMono, IAnimationModule
    {
        private Animator _animator;
        
        public Animator Animator => _animator??= GetComponent<Animator>();
    }
}