using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(IModuleAnimationController))]
    public class MovementToPrint : MonoBehaviour, IRequireMovement
    {
        [SerializeField] private AnimationChange _animationChange;
        private IModuleAnimationController _animationController;

        private void Awake()
        {
            _animationController = GetComponent<IModuleAnimationController>();
        }
        
        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            _animationChange.ApplyToAnimator(_animationController.ReferenceModule.Animator);
        }
    }
}