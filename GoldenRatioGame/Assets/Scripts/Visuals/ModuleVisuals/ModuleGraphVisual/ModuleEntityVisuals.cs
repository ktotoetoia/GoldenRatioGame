using System;
using System.Linq;
using IM.Graphs;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private ModuleGraphToVisualGraphConverter _converter;
        private IVisualModuleGraph _graphToDraw;
        private IVectorMovement _vectorMovement;
        
        private ModuleGraphToVisualGraphConverter Converter => _converter ??= new ModuleGraphToVisualGraphConverter();

        private void Awake()
        {
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_graphToDraw == null) return;

            if (_vectorMovement.CurrentMovementDirection.x != 0)
                _graphToDraw.Transform.LocalScale = _vectorMovement.CurrentMovementDirection.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            
            _graphToDraw.Transform.Position = transform.position;
            
            foreach (IAnimationModule module in _graphToDraw.Modules.OfType<IAnimationModule>())
                module.Animator.SetBool(IsRunning, true);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            (_graphToDraw as IDisposable)?.Dispose();
            
            Converter.Position = transform.position;
            _graphToDraw = Converter.Create(graph);
        }
    }
}