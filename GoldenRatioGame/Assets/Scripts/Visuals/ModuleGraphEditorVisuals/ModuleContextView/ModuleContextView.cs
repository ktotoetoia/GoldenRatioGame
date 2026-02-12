using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    [DefaultExecutionOrder(ModuleContextViewExecutionOrder)]
    public class ModuleContextView : MonoBehaviour, IModuleContextView
    {
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModuleGraphView _graphView;
        private IModuleContextInput _input;
        private IModuleEditingContext _moduleEditingContext;

        private const int ModuleContextViewExecutionOrder = 10000;
        
        private void Awake()
        {
            if(!TryGetComponent(out _input)) throw new ArgumentException("GameObject does not contain ModuleContextInput");
        }

        private void Update()
        {
            if(_moduleEditingContext == null) return;
            
            _graphView.Update();
        }

        public void SetModuleContext(IModuleEditingContext moduleEditingContext)
        {
            if(_moduleEditingContext != null) throw new Exception("Module entity has already been set");
            
            _moduleEditingContext = moduleEditingContext;
            _input.SetGraph(moduleEditingContext.GraphEditor.StartEditing());
            _graphView.SetGraph(_moduleEditingContext.GraphEditor.Graph);
            _storageView.SetStorage(moduleEditingContext.Storage);
        }

        public void ClearModuleContext()
        {
            if(_moduleEditingContext == null) return;
            if(!_moduleEditingContext.GraphEditor.TrySaveChanges()) _moduleEditingContext.GraphEditor.CancelChanges();
            
            _moduleEditingContext = null;
            _input.ClearGraph();
            _graphView.ClearGraph();
            _storageView.ClearStorage();
        }
    }
}