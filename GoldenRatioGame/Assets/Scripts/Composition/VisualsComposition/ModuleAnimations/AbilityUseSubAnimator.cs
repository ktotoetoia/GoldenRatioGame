using UnityEngine;

namespace IM.Visuals
{
    public class AbilityUseSubAnimator : MonoBehaviour, IModuleSubAnimator
    {
        [SerializeField] private string _abilityTimeParameter;
        [SerializeField] private string _abilityUsedTriggerParameter;
        [SerializeField] private Animator _animator;
        private float _stopOnTime = 0f;

        public IAnimationContext CurrentAnimationContext { get; private set; }
        
        private void Update()
        {
            if (CurrentAnimationContext != null && _stopOnTime <= Time.time)
            {
                Stop();
            }
        }

        public bool CanPlay(IAnimationContext context)
        {
            return context is AbilityAnimationContext;
        }

        public void Play(IAnimationContext context)
        {
            if(context is not AbilityAnimationContext abilityContext) return;

            _stopOnTime = Time.time + abilityContext.FocusTime;
            CurrentAnimationContext = context;
            
            _animator.SetTrigger(_abilityUsedTriggerParameter);
            _animator.SetFloat(_abilityTimeParameter, 1/abilityContext.FocusTime);
        }

        public void Stop()
        {
            CurrentAnimationContext = null;
            _animator.SetFloat(_abilityTimeParameter, 1);
        }
    }
}