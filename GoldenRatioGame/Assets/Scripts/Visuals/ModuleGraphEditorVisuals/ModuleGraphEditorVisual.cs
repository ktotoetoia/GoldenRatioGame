using IM.Graphs;
using IM.Storages;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorVisual : IModuleGraphEditorVisual
    {
        public IModuleGraphEditor<IConditionalCommandModuleGraph> ModuleGraphEditor { get; private set; }
        public IStorage Source { get; private set; }
        
        public void StartEditing(IModuleGraphEditor<IConditionalCommandModuleGraph> moduleGraphEditor, IStorage source)
        {
            ModuleGraphEditor = moduleGraphEditor;
            Source = source;
        } 
    }
}