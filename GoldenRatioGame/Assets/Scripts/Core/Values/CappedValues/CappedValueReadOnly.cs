namespace IM.Values
{
    public readonly struct CappedValueReadOnly<T> : ICappedValueReadOnly<T>
    {
        public T MinValue { get; }
        public T MaxValue { get; }
        public T Value { get; }

        public CappedValueReadOnly(T value, T minValue, T maxValue)
        {
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}