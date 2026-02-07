using System;

namespace IM.Modules
{
    public interface IEnumStateExtension< TEnum> : IExtension where TEnum : struct, Enum
    {
        TEnum Value { get; set; }
        event Action<TEnum> ValueChanged;
    }
}