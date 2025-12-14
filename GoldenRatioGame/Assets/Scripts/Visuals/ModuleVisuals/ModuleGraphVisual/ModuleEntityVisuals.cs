using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        private ModuleGraphToVisualGraphConvertor _c;
        private IVisualModuleGraph _graphToDraw;

        private ModuleGraphToVisualGraphConvertor Convertor => _c ??= new ModuleGraphToVisualGraphConvertor();

        private void Update()
        {
            if (_graphToDraw != null) _graphToDraw.Transform.Position = transform.position;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            UpdateGraphToDraw(graph);
        }

        private void UpdateGraphToDraw(IModuleGraphReadOnly graph)
        {
            (_graphToDraw as IDisposable)?.Dispose();
            
            Convertor.Position = transform.position;
            _graphToDraw = Convertor.Create(graph);
        }
    }
}