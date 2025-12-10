using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        [SerializeField] private GameObject _visualModulePrefab;
        private ModuleGraphToVisualGraphConvertor _c;
        private IVisualModuleGraph _graphToDraw;

        private ModuleGraphToVisualGraphConvertor Convertor =>
            _c ??=
                new ModuleGraphToVisualGraphConvertor(
                    new ModuleLayoutToVisualModuleMonoConvertor(_visualModulePrefab, transform));

        private void Update()
        {
            _graphToDraw.Transform.Position = transform.position;
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