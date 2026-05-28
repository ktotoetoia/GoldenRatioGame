using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Effects
{
    public class FloatTemporaryEffectGroup : ITemporaryEffectGroup
    {
        private readonly float _startTime;
        private readonly float _duration;
        
        public ITypeRegistry<IEffectModifier> Modifiers { get; }
        public float EstimatedDuration => _duration;
        public float EstimatedTimeLeft => Mathf.Max(0, _duration - (Time.time - _startTime));
        public bool IsFinished => Time.time - _startTime >= _duration;
        public FloatTemporaryEffectGroup(IEnumerable<IEffectModifier> modifiers,float time) : this(new TypeRegistry<IEffectModifier>(modifiers),time)
        {
            
        }
        
        public FloatTemporaryEffectGroup(ITypeRegistry<IEffectModifier> modifiers,float time)
        {
            Modifiers = modifiers;
            _duration = time;
            _startTime = Time.time;
        }
    }
}