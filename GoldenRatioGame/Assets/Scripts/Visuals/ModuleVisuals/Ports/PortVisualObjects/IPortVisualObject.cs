namespace IM.Visuals
{
    public interface IPortVisualObject : IVisualObject
    {
        IModuleVisualObject OwnerVisualObject { get; }
        int OutputOrderAdjustment { get; }
        bool Highlighted { get; set; }
        
        void Reset();
    }
}