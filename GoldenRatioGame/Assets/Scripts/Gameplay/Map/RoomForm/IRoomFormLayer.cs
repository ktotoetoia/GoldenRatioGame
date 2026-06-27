namespace IM.Map
{
    public interface IRoomFormLayer
    {
        void Apply(IRoomShape shape);
        void SetupPort(PortTileInfo portInfo);
    }
}