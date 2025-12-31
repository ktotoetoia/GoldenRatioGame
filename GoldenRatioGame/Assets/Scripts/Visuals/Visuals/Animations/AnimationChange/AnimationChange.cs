using System;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class AnimationChange : IAnimationChange
    {
        [SerializeField] private AnimationChangeType _animationChangeType;
        [SerializeField] private string _parameterName;
        [SerializeField] private bool _boolValue;
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;

        public AnimationChangeType AnimationChangeType => _animationChangeType;
        public string ParameterName => _parameterName;

        public void ApplyToAnimator(Animator animator)
        {
            switch (_animationChangeType)
            {
                case AnimationChangeType.Bool:
                    animator.SetBool(_parameterName, _boolValue);
                    break;

                case AnimationChangeType.Int:
                    animator.SetInteger(_parameterName, _intValue);
                    break;

                case AnimationChangeType.Float:
                    animator.SetFloat(_parameterName, _floatValue);
                    break;

                case AnimationChangeType.Trigger:
                    animator.SetTrigger(_parameterName);
                    break;

                case AnimationChangeType.None:
                    break;
            }
        }
    }
}