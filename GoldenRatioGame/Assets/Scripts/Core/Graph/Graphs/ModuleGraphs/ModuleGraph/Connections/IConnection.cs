namespace IM.Graphs
{
    public interface IConnection : IEdge
    {
        IPort Input { get; }
        IPort Output { get; }
        
        IPort GetOtherPort(IPort port);
    }
}