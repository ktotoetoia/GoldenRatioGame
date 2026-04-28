using IM.Items;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public interface IExtensibleItem : IItem, IStorable, IEntity
    {
        ITypeRegistry<IExtension> Extensions { get; }
        IPortFactory PortFactory { get; }
    }
}