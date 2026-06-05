using System;
using IM.Abilities;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class FocusDirectionVelocityEffectModifierFactory : IEffectModifierFactory
    {
        [SerializeField] private float _magnitude = 3;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _degreesOffset;

        public IEffectModifier Create(IEffectContext context)
        {
            Vector3 baseDirection =
                context.Instigator.gameObject.GetComponent<IFocusProvider>().GetFocusDirection();
            Quaternion rotation = Quaternion.AngleAxis(_degreesOffset, Vector3.forward);
            Vector3 modifiedDirection = rotation * baseDirection;

            return new VelocityEffectModifier(modifiedDirection, _curve, Time.time, _magnitude, _duration);
        }
    }
}