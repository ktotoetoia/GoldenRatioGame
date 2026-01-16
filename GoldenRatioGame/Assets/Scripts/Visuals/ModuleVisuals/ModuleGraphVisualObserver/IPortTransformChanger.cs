using IM.Modules;

namespace IM.Visuals
{
    public interface IPortTransformChanger : IModuleExtension
    {
        IModuleGraphStructureUpdater ModuleGraphStructureUpdater { get; set; }
    }
}