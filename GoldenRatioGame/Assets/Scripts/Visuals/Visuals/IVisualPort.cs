using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualPort : IPort, IHavePosition, IHaveRelativePosition
    {
        IVisualModule VisualModule { get; }
    }
}