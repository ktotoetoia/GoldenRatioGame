using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualConnection : IConnection
    {
        new IVisualPort Input { get; }
        new IVisualPort Output { get; }   
    }
}