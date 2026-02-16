using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimateOnMove : MonoBehaviour, IRequireMovement, IAnimationChange
    {
        private Vector2 _velocity;

        [field: SerializeField] public AnimationChangeType AnimationChangeType { get; private set; }
        [field: SerializeField] public string ParameterName { get; private set; }

        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public void ApplyToAnimator(Animator animator)
        {
            if (!animator.isActiveAndEnabled) return;

            switch (AnimationChangeType)
            {
                case AnimationChangeType.Bool:
                    animator.SetBool(ParameterName, _velocity.magnitude != 0);
                    break;
                case AnimationChangeType.Int:
                    animator.SetInteger(ParameterName, (int)_velocity.magnitude);
                    break;
                case AnimationChangeType.Float:
                    animator.SetFloat(ParameterName, _velocity.magnitude);
                    break;
                case AnimationChangeType.Trigger:
                    if (_velocity.magnitude != 0) animator.SetTrigger(ParameterName);
                    break;
                default:
                case AnimationChangeType.None:
                    break;
            }
        }
    }
}