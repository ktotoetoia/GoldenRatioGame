using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisual : IModuleExtension
    {
        IModuleVisualObject CreateModuleVisualObject();
    }
}