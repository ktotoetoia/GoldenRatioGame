using System;

namespace TDS.Events
{
    public interface IValueStorage<T>
    {
        T Value { get; set; }

        event Action<T> Changed;
    }
}