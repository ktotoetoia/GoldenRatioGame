using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorVisual : MonoBehaviour, IModuleGraphEditorVisual
    {
        public IModuleGraphEditor<IConditionalCommandModuleGraph> ModuleGraphEditor { get; private set; }
        public ICellFactoryStorage Source { get; private set; }
        
        public void StartEditing(IModuleGraphEditor<IConditionalCommandModuleGraph> moduleGraphEditor, ICellFactoryStorage source)
        {
            ModuleGraphEditor = moduleGraphEditor;
            Source = source;
        }

        public void StopEditing()
        {
            
        }
    }
}