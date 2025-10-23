using IM.Graphs;

namespace IM.Modules
{
    public interface IGameModule : IModule
    {
        IModuleExtensions Extensions { get; }
        
        IModuleLayout GetModuleLayout();
        IModuleRenderer GetModuleRenderer();
    }
}