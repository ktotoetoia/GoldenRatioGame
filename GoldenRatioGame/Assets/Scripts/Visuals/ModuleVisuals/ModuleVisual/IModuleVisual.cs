using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisual : IModuleExtension
    {
        IIcon Icon { get; }
        
        IModuleVisualObject CreateModuleVisualObject();
    }
}