using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleAnimator : MonoBehaviour, IModuleAnimator
    {
        [SerializeField] private GameObject _subAnimatorsSource;
        private List<IModuleSubAnimator> _subAnimators;
        
        private List<IModuleSubAnimator> SubAnimators => _subAnimators ??=_subAnimatorsSource.GetComponents<IModuleSubAnimator>().ToList();
        public IEnumerable<IAnimationContext> CurrentAnimationContexts => SubAnimators.Select(x => x.CurrentAnimationContext).Where(x => x != null);
        
        public bool CanPlay(IAnimationContext context) => SubAnimators.Any(x => x.CanPlay(context));

        public void Play(IAnimationContext context)
        {
            foreach (var subAnimator in SubAnimators.Where(subAnimator => subAnimator.CanPlay(context)))
            {
                subAnimator.Play(context);
            }
        }

        public void Stop(IAnimationContext context)
        {
            foreach (IModuleSubAnimator moduleAnimator in SubAnimators)
            {
                if (moduleAnimator.CurrentAnimationContext == context)
                {
                    moduleAnimator.Stop();
                }
            }
        }
    }
}