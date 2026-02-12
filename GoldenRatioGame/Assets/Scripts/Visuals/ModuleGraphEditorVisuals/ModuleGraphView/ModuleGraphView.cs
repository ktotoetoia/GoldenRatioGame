using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphView : MonoBehaviour
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModuleGraphReadOnly _graph;
        private ModuleGraphVisualObserver _visualObserver;
        
        public void SetGraph(IModuleGraphReadOnly graph)
        {
            if (_visualObserver != null) throw new InvalidOperationException();
            
            _visualObserver = new ModuleGraphVisualObserver(transform,false,_preset);
            _graph = graph;   
        }

        public void Update()
        {
            if (_visualObserver == null || _graph == null) return;
            
            _visualObserver.OnGraphUpdated(_graph);
        }

        public void ClearGraph()
        {
            _visualObserver.Dispose();
            _visualObserver = null;
            _graph = null;
        }
    }
}