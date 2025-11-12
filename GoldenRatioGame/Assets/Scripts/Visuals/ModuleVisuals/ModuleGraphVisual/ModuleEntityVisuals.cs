using System;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleEntityVisuals, IModuleGraphObserver
    {
        private IModuleGraphVisualDrawer _graphVisualDrawer;
        private IVisualModuleGraph _graphToDraw;
        private Vector3 _lastPosition;
        
        private void Awake()
        {
            _graphVisualDrawer = new ModuleGraphVisualDrawer();
        }

        private void Update()
        {
            Vector3 diff = transform.position - _lastPosition;

            foreach (IVisualModule module in _graphToDraw.Modules)
            {
                module.Transform.Position += diff;
            }
            
            _lastPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            if(_graphVisualDrawer == null || _graphToDraw == null) return;
            
            _graphVisualDrawer.Draw(_graphToDraw);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _graphToDraw = new ModuleGraphToVisualGraphConvertor().Create(graph);
        }
    }
}