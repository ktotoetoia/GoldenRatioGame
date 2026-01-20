using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisual : IExtension
    {
        IModuleVisualObject CreateModuleVisualObject();
    }
}