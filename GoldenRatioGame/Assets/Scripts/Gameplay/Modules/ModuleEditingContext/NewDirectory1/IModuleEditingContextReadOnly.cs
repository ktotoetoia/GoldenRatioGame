using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public interface IModuleEditingContextReadOnly
    {
        IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }
        IReadOnlyStorage Storage { get; }
    }
}