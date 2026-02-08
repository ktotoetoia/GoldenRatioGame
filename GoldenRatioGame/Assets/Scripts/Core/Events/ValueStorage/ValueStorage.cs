using System;

namespace TDS.Events
{
    public class ValueStorage<T> : IValueStorage<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set 
            {
                if(value.Equals(_value)) return;
                
                _value = value;
                
                Changed?.Invoke(value);
            }
        }
        
        public event Action<T> Changed;
    }
}