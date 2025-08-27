using System;
using System.Collections.Generic;

namespace IM.Economy
{
    public interface ICappedValue<T> : ICappedValueReadOnly<T>
    {
        new T MinValue { get; set; }
        new T MaxValue { get; set; }
        new T Value { get; set; }
    }
}