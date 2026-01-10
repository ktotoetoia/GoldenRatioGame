using IM.Graphs;

namespace IM.Visuals
{
    public interface ITransformConnection : IConnection
    {
        new ITransformPort Port1 { get; }
        new ITransformPort Port2 { get; }   
    }
}