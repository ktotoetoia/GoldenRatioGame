using System.Linq;

namespace IM.Visuals
{
    public class VisualGraphAnimator : IAnimator
    {
        private readonly IVisualModuleGraph _graph;
        
        public VisualGraphAnimator(IVisualModuleGraph graph)
        {
            _graph = graph;
        }

        public void SetBool(string propertyName, bool value)
        {
            foreach (IAnimationModule module in _graph.Modules.OfType<IAnimationModule>())
            {
                module.Animator.SetBool(propertyName, value);
            }
        }
        
        public void SetTrigger(string propertyName)
        {
            foreach (IAnimationModule module in _graph.Modules.OfType<IAnimationModule>())
            {
                module.Animator.SetTrigger(propertyName);
            }
        }

        public void SetFloat(string propertyName, float value)
        {
            foreach (IAnimationModule module in _graph.Modules.OfType<IAnimationModule>())
            {
                module.Animator.SetFloat(propertyName, value);
            }
        }

        public void SetInteger(string propertyName, int value)
        {
            foreach (IAnimationModule module in _graph.Modules.OfType<IAnimationModule>())
            {
                module.Animator.SetInteger(propertyName, value);
            }
        }
    }
}