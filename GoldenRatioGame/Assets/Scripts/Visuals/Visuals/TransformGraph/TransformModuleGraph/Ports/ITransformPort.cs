using IM.Graphs;

namespace IM.Visuals
{
    public interface ITransformPort : IPort
    {
        IHierarchyTransform Transform { get; }
        new ITransformModule Module { get; }
        new ITransformConnection Connection { get; }
    }
}