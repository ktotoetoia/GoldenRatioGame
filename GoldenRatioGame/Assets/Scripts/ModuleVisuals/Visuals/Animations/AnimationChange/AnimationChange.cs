using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace IM.Visuals
{
    [Serializable]
    public class AnimationChange : IAnimationChange
    {
        [FormerlySerializedAs("_animationChangeType")] [SerializeField] private ParameterType parameterType;
        [SerializeField] private string _parameterName;
        [SerializeField] private bool _boolValue;
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;

        public ParameterType ParameterType => parameterType;
        public string ParameterName => _parameterName;

        public void ApplyToAnimator(Animator animator)
        {
            switch (parameterType)
            {
                case ParameterType.Bool:
                    animator.SetBool(_parameterName, _boolValue);
                    break;

                case ParameterType.Int:
                    animator.SetInteger(_parameterName, _intValue);
                    break;

                case ParameterType.Float:
                    animator.SetFloat(_parameterName, _floatValue);
                    break;

                case ParameterType.Trigger:
                    animator.SetTrigger(_parameterName);
                    break;

                case ParameterType.None:
                    break;
            }
        }
    }
}