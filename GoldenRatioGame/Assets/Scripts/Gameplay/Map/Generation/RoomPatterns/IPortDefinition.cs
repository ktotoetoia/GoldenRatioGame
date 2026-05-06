namespace IM.Map
{
    public interface IPortDefinition
    {
        PortSide Side { get; }
        float NormalizedPosition { get; }
    }
}