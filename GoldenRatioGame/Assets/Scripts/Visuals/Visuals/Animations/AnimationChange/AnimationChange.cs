using System;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class AnimationChange : IAnimationChange
    {
        [SerializeField] private AnimationChangeType _animationChangeType;
        [SerializeField] private string _propertyName;
        [SerializeField] private bool _boolValue;
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;

        public AnimationChangeType AnimationChangeType => _animationChangeType;
        public string PropertyName => _propertyName;

        public void ApplyToAnimator(Animator animator)
        {
            switch (_animationChangeType)
            {
                case AnimationChangeType.Bool:
                    animator.SetBool(_propertyName, _boolValue);
                    break;

                case AnimationChangeType.Int:
                    animator.SetInteger(_propertyName, _intValue);
                    break;

                case AnimationChangeType.Float:
                    animator.SetFloat(_propertyName, _floatValue);
                    break;

                case AnimationChangeType.Trigger:
                    animator.SetTrigger(_propertyName);
                    break;

                case AnimationChangeType.None:
                    break;
            }
        }
    }
}

