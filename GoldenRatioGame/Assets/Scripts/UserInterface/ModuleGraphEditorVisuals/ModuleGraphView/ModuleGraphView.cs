using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphView : MonoBehaviour
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModuleGraphReadOnly _graph;

        public ModuleGraphVisualObserver VisualObserver { get; private set; }

        public void SetGraph(IModuleGraphReadOnly graph)
        {
            if (VisualObserver != null) throw new InvalidOperationException();

            VisualObserver = new ModuleGraphVisualObserver(transform, false, _preset)
            {
                ShowPortsOnConnected = false,
                ShowPortsOnDisconnected = true
            };
            _graph = graph;   
        }

        public void Update()
        {
            if (VisualObserver == null || _graph == null) return;
            
            VisualObserver.OnGraphUpdated(_graph);
        }

        public void ClearGraph()
        {
            VisualObserver.Dispose();
            VisualObserver = null;
            _graph = null;
        }
    }
}