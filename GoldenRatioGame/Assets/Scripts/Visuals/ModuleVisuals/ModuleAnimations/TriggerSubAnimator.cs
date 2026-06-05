using UnityEngine;

namespace IM.Visuals
{
    public class TriggerSubAnimator : MonoBehaviour, IModuleSubAnimator
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _contextName;
        [SerializeField] private string _parameterName;

        public IAnimationContext CurrentAnimationContext => null;

        public bool CanPlay(IAnimationContext context)
        {
            return context is TriggerAnimationContext tr && tr.Name == _parameterName; 
        }

        public void Play(IAnimationContext context)
        {
            if(context is not TriggerAnimationContext tr || tr.Name != _parameterName) return;
            
            _animator.SetTrigger(_contextName);
        }

        public void Stop()
        {
            
        }
    }
}