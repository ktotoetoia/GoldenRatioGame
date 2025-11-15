using System;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleEntityVisuals, IModuleGraphObserver
    {
        [SerializeField] private bool _drawBounds = true;
        [SerializeField] private bool _drawSprites = true;
        [SerializeField] private bool _drawPorts = true;
        
        private readonly ModuleGraphToVisualGraphConvertor _graphToVisualGraphConvertor = new();
        private ModuleGraphVisualDrawer _graphVisualDrawer;
        private IVisualModuleGraph _graphToDraw;
        private Vector3 _lastPosition;
        
        private void Awake()
        {
            _graphVisualDrawer = new ModuleGraphVisualDrawer();
        }

        private void Update()
        {
            _graphVisualDrawer.DrawBounds = _drawBounds;
            _graphVisualDrawer.DrawSprites = _drawSprites;
            _graphVisualDrawer.DrawPorts = _drawPorts;
            
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

            _graphToVisualGraphConvertor.Position = transform.position;
            _graphToDraw = _graphToVisualGraphConvertor.Create(graph);
        }
    }
}