using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(IModuleAnimationController))]
    public class MovementToPrint : MonoBehaviour, IRequireMovement
    {
        [SerializeField] private string _boolName;
        private IModuleAnimationController _animationController;

        private void Awake()
        {
            _animationController = GetComponent<IModuleAnimationController>();
        }
        
        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            if(_animationController?.ReferenceModule == null) return;
            
            _animationController.ReferenceModule.Animator.SetBool(_boolName, velocity != Vector2.zero);
        }
    }
}