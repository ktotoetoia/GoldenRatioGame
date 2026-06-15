namespace IM.Visuals
{
    public interface IPortVisualObject : IVisualObject
    {
        IModuleVisualObject OwnerVisualObject { get; }
        public int Rotation { get; set; }
        int OutputOrderAdjustment { get; }
        
        void Reset();
    }
}