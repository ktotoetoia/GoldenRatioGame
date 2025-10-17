using IM.Graphs;

namespace IM.Modules
{
    public interface IGameModule : IModule
    {
        IModuleLayout Layout { get; }
        IModuleContextExtensions Extensions { get; }
    }
}