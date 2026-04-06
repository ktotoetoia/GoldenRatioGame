using IM.Graphs;
using IM.Items;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public interface IExtensibleModule : IModule, IItem, IStorable, IEntity
    {
        ITypeRegistry<IExtension> Extensions { get; }
        ModuleState ModuleState { get; set; }
        
        int GetPortId(IPort port);
        IPort GetPort(int id);
    }
}