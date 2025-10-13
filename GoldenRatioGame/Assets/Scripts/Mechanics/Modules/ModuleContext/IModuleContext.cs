using IM.Base;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleContext
    {
        IModuleContextExtensions Extensions { get; }
        IModule GetModule();
    }
}