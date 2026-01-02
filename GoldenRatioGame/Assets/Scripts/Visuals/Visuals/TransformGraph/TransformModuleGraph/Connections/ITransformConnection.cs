using IM.Graphs;

namespace IM.Visuals
{
    public interface ITransformConnection : IConnection
    {
        new ITransformPort Input { get; }
        new ITransformPort Output { get; }   
    }
}