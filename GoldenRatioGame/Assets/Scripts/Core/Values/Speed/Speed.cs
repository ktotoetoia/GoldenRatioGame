using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Values
{
    public class Speed : ISpeed
    {
        private List<ISpeedModifier> _modifiers = new();

        [field: SerializeField] public float RawValue { get; set; }
        public float FinalValue => RawValue + _modifiers.Sum(x => x.GetModifiedValue(RawValue));
        public IReadOnlyList<ISpeedModifier> Modifiers => _modifiers;

        public Speed(float rawValue = 0) => RawValue = rawValue;

        public void AddModifier(ISpeedModifier modifier) => _modifiers.Add(modifier);
        public void RemoveModifier(ISpeedModifier modifier) => _modifiers.Remove(modifier);
    }
}