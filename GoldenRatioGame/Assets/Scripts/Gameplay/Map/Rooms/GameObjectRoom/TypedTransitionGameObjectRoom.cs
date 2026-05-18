using UnityEngine;

namespace IM.Map
{
    public class TypedTransitionGameObjectRoom : TypedGameObjectRoom, IHaveTransitionPoint
    {
        [SerializeField] private GameObject _transitionPointSource;
        private ITransitionPoint _transitionPoint;
        
        public ITransitionPoint TransitionPoint =>  _transitionPoint ??= _transitionPointSource.GetComponent<ITransitionPoint>();
    }
}