using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualPort : IPort, IHavePosition, IHaveRelativePosition
    {
        new IVisualModule Module { get; }
        new IVisualConnection Connection { get; }
    }
}