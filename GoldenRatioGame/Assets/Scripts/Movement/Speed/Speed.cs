using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Movement
{
    [Serializable]
    public class Speed : ISpeed
    {
        [field: SerializeField] public float RawValue { get; set; }
        public IList<ISpeedModifier> Modifiers { get; }
        public float FinalValue => RawValue + Modifiers.Sum(x => x.GetModifiedValue(RawValue));

        public Speed() : this(0)
        {

        }

        public Speed(float rawValue)
        {
            RawValue = rawValue;
            Modifiers = new List<ISpeedModifier>();
        }
    }
}