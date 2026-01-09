namespace IM.Graphs
{
    public interface IConnection : IEdge
    {
        IPort Port1 { get; }
        IPort Port2 { get; }
        
        IPort GetOtherPort(IPort port);
    }
}