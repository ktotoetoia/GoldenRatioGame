using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IPortVisualObject 
    {
        IModuleVisualObject OwnerVisualObject { get; }
        IPort Port { get; }
        ITransform Transform { get; }
    }
}