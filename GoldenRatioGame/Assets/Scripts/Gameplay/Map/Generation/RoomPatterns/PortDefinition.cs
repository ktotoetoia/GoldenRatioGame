namespace IM.Map
{
    public class PortDefinition : IPortDefinition
    {
        public PortSide Side { get; }
        public float NormalizedPosition { get; }
        
        public PortDefinition(PortSide side, float normalizedPosition = 0.5f)
        {
            Side = side;
            NormalizedPosition = normalizedPosition;
        }
    }
}