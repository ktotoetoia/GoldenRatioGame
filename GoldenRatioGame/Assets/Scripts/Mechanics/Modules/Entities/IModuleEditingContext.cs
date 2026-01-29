using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public interface IModuleEditingContext
    {
        IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; }
        IReadOnlyStorage Storage { get; }
        void AddToStorage(IExtensibleModule module);
        void RemoveFromStorage(IExtensibleModule module);
    }
}