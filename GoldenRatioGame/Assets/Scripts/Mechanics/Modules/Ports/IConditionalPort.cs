using IM.Graphs;

namespace IM.Modules
{
    public interface IConditionalPort : IPort
    {
        bool CanConnect(IPort other);
        bool CanDisconnect();
    }
}