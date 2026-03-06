using System.Collections.Generic;

namespace IM.Common
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