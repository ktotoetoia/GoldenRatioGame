using System.Collections.Generic;
using System.Linq;

namespace IM.Values
{
    public class Speed : ISpeed
    {
        private readonly List<ISpeedModifier> _modifiers = new();

        public IReadOnlyList<ISpeedModifier> Modifiers => _modifiers;
        public float RawValue { get; set; }
        public float FinalValue
        {
            get
            {
                float afterAddition = RawValue + _modifiers.Sum(x => x.Add);
                float multiplication = 1;

                foreach(ISpeedModifier modifier in _modifiers)
                {
                    multiplication *= modifier.Multiplication;
                }

                return afterAddition * multiplication;
            }
        }

        public Speed(float rawValue = 0) => RawValue = rawValue;

        public void AddModifier(ISpeedModifier modifier) => _modifiers.Add(modifier);
        public void RemoveModifier(ISpeedModifier modifier) => _modifiers.Remove(modifier);
    }
}