using IM.Graphs;
using IM.Items;
using IM.Storages;

namespace IM.Modules
{
    public interface IExtensibleModule : IModule, IItem, IStorable
    {
        ITypeRegistry<IExtension> Extensions { get; }
        ModuleState ModuleState { get; set; }
    }
}