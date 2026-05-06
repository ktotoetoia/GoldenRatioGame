namespace IM.Map
{
    public class PortDefinition : IPortDefinition
    {
        public PortSide Side { get; }
        public float NormalizedPosition { get; }
        
        public PortDefinition(PortSide side, float normalizedPosition)
        {
            Side = side;
            NormalizedPosition = normalizedPosition;
        }
    }
}