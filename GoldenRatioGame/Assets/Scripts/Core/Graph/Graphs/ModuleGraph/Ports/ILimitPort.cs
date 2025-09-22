namespace IM.Graphs
{
    public interface ILimitPort : IModulePort
    {
        bool CanConnect(IModulePort other);
        bool CanDisconnect();
    }
}