using IM.Graphs;
using IM.Storages;

namespace IM.Visuals.Graph
{
    public interface IModuleGraphEditorVisual
    {
        IModuleGraphEditor<IConditionalCommandModuleGraph> ModuleGraphEditor { get; }
        ICellFactoryStorage Source { get; }

        void StartEditing(IModuleGraphEditor<IConditionalCommandModuleGraph> moduleGraphEditor,
            ICellFactoryStorage source);
    }
}