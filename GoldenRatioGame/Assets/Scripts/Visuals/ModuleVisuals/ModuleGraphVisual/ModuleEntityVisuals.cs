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
        [SerializeField] private bool _flip;
        [SerializeField] private Vector3 _position = Vector3.one;
        [SerializeField] private Vector3 _scale = Vector3.one;
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
            
            _graphToDraw.Transform.Position = transform.position;
            _graphToDraw.Transform.Scale = _scale;
        }

        private void OnDrawGizmos()
        {
            if(_graphVisualDrawer == null || _graphToDraw == null) return;
            
            _graphVisualDrawer.Draw(_graphToDraw);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _graphToDraw = _graphToVisualGraphConvertor.Create(graph);
        }
    }
}