using UnityEngine;

namespace IM.Visuals
{
    public class MoveSubAnimator : MonoBehaviour, IModuleSubAnimator
    {
        [SerializeField] private string _offsetParameterName;
        [SerializeField] private string _movingParameterName;
        [SerializeField] private Animator _animator;
        private int _lastFramePlayed = 0;
        private const int _framesToStop = 1;
        public IAnimationContext CurrentAnimationContext { get; private set; }

        private void Update()
        {
            if (CurrentAnimationContext != null && Time.frameCount > _lastFramePlayed + _framesToStop)
            {
                Stop();
            }   
        }
        
        public bool CanPlay(IAnimationContext context)
        {
            return context is MoveAnimationContext;
        }

        public void Play(IAnimationContext context)
        {
            if(context is not MoveAnimationContext moveContext) return;

            _lastFramePlayed = Time.frameCount;
            CurrentAnimationContext = moveContext;
            
            if (moveContext.Velocity.magnitude <= 0.01f)
            {
                Stop();
                return;
            }
            
            _animator.SetFloat(_offsetParameterName, moveContext.Offset);
            _animator.SetBool(_movingParameterName,true);
        }

        public void Stop()
        {
            CurrentAnimationContext = null;
            _animator.SetFloat(_offsetParameterName, 0);
            _animator.SetBool(_movingParameterName,false);
        }
    }
}