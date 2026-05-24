namespace IM.Map
{
    public class PortDefinition : IPortDefinition
    {
        public PortSide Side { get; }
        public float NormalizedPosition { get; }
        public int Index { get; }

        public PortDefinition(PortSide side, int index, float normalizedPosition = 0.5f)
        {
            Side = side;
            Index = index;
            NormalizedPosition = normalizedPosition;
        }
    }
}