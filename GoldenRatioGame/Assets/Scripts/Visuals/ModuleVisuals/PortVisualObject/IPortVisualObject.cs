using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IPortVisualObject : IVisualObject
    {
        IModuleVisualObject OwnerVisualObject { get; }
        IPort Port { get; }
        ITransform Transform { get; }
        int OutputOrderAdjustment { get; }
        
        void Reset();
    }
}