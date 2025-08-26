using System.Collections.Generic;

namespace TD.Movement
{
    public interface ISpeed
    {
        public float RawValue { get; set; }
        public float FinalValue { get; }
        public IList<ISpeedModifier> Modifiers { get; }
    }
}