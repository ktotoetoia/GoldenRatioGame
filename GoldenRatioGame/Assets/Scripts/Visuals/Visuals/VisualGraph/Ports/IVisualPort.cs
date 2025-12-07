using IM.Graphs;

namespace IM.Visuals
{
    public interface IVisualPort : IPort
    {
        IHierarchyTransform Transform { get; }
        new IVisualModule Module { get; }
        new IVisualConnection Connection { get; }
    }
}