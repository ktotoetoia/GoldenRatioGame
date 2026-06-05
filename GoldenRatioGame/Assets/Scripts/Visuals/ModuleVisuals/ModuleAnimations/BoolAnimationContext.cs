namespace IM.Visuals
{
    public class BoolAnimationContext : IAnimationContext
    {
        public bool Value { get; }
        public string Name { get; }
        
        public BoolAnimationContext( string name,bool value)
        {
            Name = name;
            Value = value;
        }
    }
}