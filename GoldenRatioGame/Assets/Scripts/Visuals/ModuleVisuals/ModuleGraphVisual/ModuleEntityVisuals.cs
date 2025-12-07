using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    [DefaultExecutionOrder(-1)]
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        [SerializeField] private GameObject _visualModulePrefab;
        private ModuleGraphToVisualGraphConvertor _graphToVisualGraphConvertor;
        private IVisualModuleGraph _graphToDraw;

        private void Awake()
        {
            _graphToVisualGraphConvertor = new ModuleGraphToVisualGraphConvertor(new ModuleLayoutToVisualModuleMonoConvertor(_visualModulePrefab,transform));
        }

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
            _graphToVisualGraphConvertor.Position = transform.position;
            _graphToDraw?.Dispose();
            
            _graphToDraw = _graphToVisualGraphConvertor.Create(graph);
        }
    }
}