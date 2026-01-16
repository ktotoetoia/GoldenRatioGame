using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisual : IModuleExtension
    {
        IModuleVisualObject ModuleVisualObject { get; }
        IIcon Icon { get; }
        
        void ResetReferenceModule();
    }
}