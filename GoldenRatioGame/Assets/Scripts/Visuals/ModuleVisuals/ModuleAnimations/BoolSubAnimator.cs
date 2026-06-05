using UnityEngine;

namespace IM.Visuals
{
    public class BoolSubAnimator : MonoBehaviour, IModuleSubAnimator
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _contextName;
        [SerializeField] private string _parameterName;
        private IAnimationContext _context;

        public IAnimationContext CurrentAnimationContext => _animator.GetBool(_parameterName) ? _context : null;
        
        public bool CanPlay(IAnimationContext context)
        {
            return context is BoolAnimationContext b&& b.Name == _contextName;
        }

        public void Play(IAnimationContext context)
        {
            if(context is not BoolAnimationContext b || b.Name != _contextName) return;

            _context = context;
            _animator.SetBool(_contextName, b.Value);
        }

        public void Stop()
        {
            _context = null;
            _animator.SetBool(_parameterName, false);
        }
    }
}