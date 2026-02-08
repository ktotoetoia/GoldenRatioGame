using System;

namespace IM.Modules
{
    public interface IValueStateExtension< TValue> : IExtension
    {
        TValue Value { get; set; }
        event Action<TValue> ValueChanged;
    }
}