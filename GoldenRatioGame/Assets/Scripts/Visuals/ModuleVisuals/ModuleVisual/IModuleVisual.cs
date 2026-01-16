using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisual : IModuleExtension
    {
        IModuleVisualObject ReferenceModuleVisualObject { get; }
        IIcon Icon { get; }
        
        IModuleVisualObject CreateModuleVisualObject();
    }
}