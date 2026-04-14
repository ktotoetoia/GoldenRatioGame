using IM.Transforms;

namespace IM.Visuals
{
    public interface IPortVisualObject : IVisualObject
    {
        IModuleVisualObject OwnerVisualObject { get; }
        ITransform Transform { get; }
        int OutputOrderAdjustment { get; }
        bool Highlighted { get; set; }
        
        void Reset();
    }
}