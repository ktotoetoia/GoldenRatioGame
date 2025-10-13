using IM.Base;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleContext : IFactory<IModule>
    {
        IModuleContextExtensions Extensions { get; }
    }
}