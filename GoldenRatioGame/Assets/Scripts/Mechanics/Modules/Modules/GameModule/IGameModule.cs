using IM.Graphs;

namespace IM.Modules
{
    public interface IGameModule : IModule
    {
        IModuleLayout Layout { get; }
        IModuleExtensions Extensions { get; }
    }
}