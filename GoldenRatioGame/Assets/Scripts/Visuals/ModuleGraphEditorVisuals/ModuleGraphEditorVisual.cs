using System;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorVisual : MonoBehaviour, IModuleGraphEditorVisual
    {
        private IConditionalCommandModuleGraph _moduleGraph;
        
        public IModuleGraphEditor<IConditionalCommandModuleGraph> ModuleGraphEditor { get; private set; }
        
        public void SetEditor(IModuleGraphEditor<IConditionalCommandModuleGraph> moduleGraphEditor)
        {
            ModuleGraphEditor = moduleGraphEditor;
            
            try
            {
                _moduleGraph = ModuleGraphEditor.StartEditing();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                
                ModuleGraphEditor = null;
            }
        }
    }
}