using System.Collections.Generic;

namespace IM.Values
{
    public interface ISpeed
    {
        float RawValue { get; set; }
        float FinalValue { get; }
        IReadOnlyList<ISpeedModifier> Modifiers { get; }

        void AddModifier(ISpeedModifier modifier);
        void RemoveModifier(ISpeedModifier modifier);
    }
}