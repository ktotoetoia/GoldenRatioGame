namespace IM.Economy
{
    public class NumerableResource<TResource> : INumerableResource<TResource>
    {
        private readonly CappedValue<int> _value;
        
        public int MinValue
        {
            get => _value.MinValue;
            set => _value.MinValue = value;
        }

        public int MaxValue
        {
            get => _value.MaxValue;
            set => _value.MaxValue = value;
        }

        public int Value
        {
            get => _value.Value;
            set => _value.Value = value;
        }
        
        public TResource Resource { get;  }

        public NumerableResource(TResource resource, int minValue = 0, int maxValue = 1,int  value = 0)
        {
            _value = new CappedValue<int>(minValue, maxValue, value);
            Resource = resource;
        }
    }
}