using System;

namespace IM.Values
{
    public interface ICappedValueReadOnlyEvents<out T> : ICappedValueReadOnly<T>
    {
        event Action<T> MinValueChanged;
        event Action<T> MaxValueChanged;
        event Action<T> ValueChanged;
    }
}