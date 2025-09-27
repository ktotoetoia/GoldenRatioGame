using System;
using UnityEngine;

namespace IM.Values
{
    [Serializable]
    public class CappedValue<T> : ICappedValue<T> where T : struct, IComparable<T>
    {
        [SerializeField]private T _minValue;
        [SerializeField]private T _maxValue;
        [SerializeField]private T _value;
        
        public event Action<T> OnMinValueChanged;
        public event Action<T> OnMaxValueChanged;
        public event Action<T> OnValueChanged;

        public T MinValue
        {
            get => _minValue;
            set
            {
                if (value.CompareTo(_maxValue) > 0)
                    throw new ArgumentException("MinValue cannot be greater than MaxValue.");
                _minValue = value;
                if (_value.CompareTo(_minValue) < 0)
                    _value = _minValue;
                
                OnMinValueChanged?.Invoke(_minValue);
            }
        }

        public T MaxValue
        {
            get => _maxValue;
            set
            {
                if (value.CompareTo(_minValue) < 0)
                    throw new ArgumentException("MaxValue cannot be less than MinValue.");
                _maxValue = value;
                if (_value.CompareTo(_maxValue) > 0)
                    _value = _maxValue;
                
                OnMaxValueChanged?.Invoke(_maxValue);
            }
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = Clamp(value);
                
                OnValueChanged?.Invoke(_value);
            }
        }

        public CappedValue(T minValue, T maxValue, T value = default)
        {
            if (minValue.CompareTo(maxValue) > 0)
                throw new ArgumentException("minValue must be less than or equal to maxValue.");

            _minValue = minValue;
            _maxValue = maxValue;
            Value = value;
        }

        public T Clamp(T value)
        {
            if (value.CompareTo(_minValue) < 0) return _minValue;
            if (value.CompareTo(_maxValue) > 0) return _maxValue;
            return value;
        }
    }
}