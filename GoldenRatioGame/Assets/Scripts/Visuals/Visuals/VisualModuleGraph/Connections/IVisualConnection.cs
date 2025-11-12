using IM.Graphs;

namespace IM.Visuals
{
    public interface IVisualConnection : IConnection
    {
        new IVisualPort Input { get; }
        new IVisualPort Output { get; }   
    }
}