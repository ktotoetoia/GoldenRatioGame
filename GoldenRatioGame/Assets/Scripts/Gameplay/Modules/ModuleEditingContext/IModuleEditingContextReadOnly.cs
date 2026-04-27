using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public interface IModuleEditingContextReadOnly
    {
        IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }
        IReadOnlyStorage Storage { get; }
        ITypeRegistry<object> Capabilities { get; }
    }
}