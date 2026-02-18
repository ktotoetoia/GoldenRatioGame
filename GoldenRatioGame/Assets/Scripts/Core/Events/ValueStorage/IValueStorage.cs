using System;

namespace IM.Events
{
    public interface IValueStorage<T>
    {
        T Value { get; set; }

        event Action<T> Changed;
    }
}