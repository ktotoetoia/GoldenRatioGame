using System;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        [SerializeField] private bool _drawBounds = true;
        [SerializeField] private bool _drawSprites = true;
        [SerializeField] private bool _drawPorts = true;
        private readonly ModuleGraphToVisualGraphConvertor _graphToVisualGraphConvertor = new();
        private readonly VisualGraphIconDrawer _visualGraphIconDrawer= new();
        private IVisualModuleGraph _graphToDraw;

        private void Update()
        {
            _visualGraphIconDrawer.DrawBounds = _drawBounds;
            _visualGraphIconDrawer.DrawSprites = _drawSprites;
            _visualGraphIconDrawer.DrawPorts = _drawPorts;
            
            _graphToDraw.Transform.Position = transform.position;
        }

        private void OnDrawGizmos()
        {
            if(_visualGraphIconDrawer == null || _graphToDraw == null) return;
            
            _visualGraphIconDrawer.Draw(_graphToDraw);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            UpdateGraphToDraw(graph);
        }

        private void UpdateGraphToDraw(IModuleGraphReadOnly graph)
        {
            _graphToVisualGraphConvertor.Position = transform.position;
            _graphToDraw = _graphToVisualGraphConvertor.Create(graph);            
        }
    }
}