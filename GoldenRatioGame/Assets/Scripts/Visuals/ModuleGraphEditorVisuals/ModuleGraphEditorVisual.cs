using IM.Graphs;
using IM.Modules;
using IM.Storages;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorVisual : MonoBehaviour, IModuleGraphEditorVisual
    {
        private IConditionalCommandModuleGraph _graph;
        private ModuleGraphVisualObserver _observer; 
        private IStorage _storage;
        
    }
}