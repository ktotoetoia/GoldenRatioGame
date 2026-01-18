using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public interface IGameModule : IModule, IItem
    {
        IModuleExtensions Extensions { get; }
    }
}