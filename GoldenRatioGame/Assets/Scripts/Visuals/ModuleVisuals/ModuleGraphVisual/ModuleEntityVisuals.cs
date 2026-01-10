using System;
using IM.Graphs;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private ModuleGraphToVisualGraphConverter _converter;
        private ITransformModuleGraph _graphToDraw;
        private IVectorMovement _vectorMovement;
        
        private ModuleGraphToVisualGraphConverter Converter => _converter ??= new ModuleGraphToVisualGraphConverter();
        public ITransformModuleGraph GraphToDraw => _graphToDraw;

        private void Awake()
        {
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_graphToDraw == null) return;

            if (_vectorMovement.MovementDirection.x != 0)
                _graphToDraw.Transform.LocalScale = _vectorMovement.MovementDirection.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            
            _graphToDraw.Transform.Position = transform.position;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            Converter.Position = transform.position;
            _graphToDraw = Converter.Create(graph);
        }
    }
}