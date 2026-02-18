using IM.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace IM.Visuals
{
    public class AnimateOnMove : MonoBehaviour, IRequireMovement, IAnimationChange
    {
        private Vector2 _velocity;

        [field: FormerlySerializedAs("<AnimationChangeType>k__BackingField")] [field: SerializeField] public ParameterType ParameterType { get; private set; }
        [field: SerializeField] public string ParameterName { get; private set; }

        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public void ApplyToAnimator(Animator animator)
        {
            if (!animator.isActiveAndEnabled) return;

            switch (ParameterType)
            {
                case ParameterType.Bool:
                    animator.SetBool(ParameterName, _velocity.magnitude != 0);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(ParameterName, (int)_velocity.magnitude);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(ParameterName, _velocity.magnitude);
                    break;
                case ParameterType.Trigger:
                    if (_velocity.magnitude != 0) animator.SetTrigger(ParameterName);
                    break;
                default:
                case ParameterType.None:
                    break;
            }
        }
    }
}