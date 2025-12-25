using System;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleEntityVisuals : MonoBehaviour, IModuleGraphObserver
    {
        private static readonly int IsWalking = Animator.StringToHash("IsRunning");
        private Rigidbody2D _rigidbody;
        private ModuleGraphToVisualGraphConverter _converter;
        private IVisualModuleGraph _graphToDraw;
        
        private ModuleGraphToVisualGraphConverter Converter => _converter ??= new ModuleGraphToVisualGraphConverter();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_graphToDraw == null) return;
            
            _graphToDraw.Transform.LocalScale = _rigidbody.linearVelocityX > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            _graphToDraw.Transform.Position = transform.position;
            
            foreach (IAnimationModule module in _graphToDraw.Modules.OfType<IAnimationModule>())
            {
                module.Animator.SetBool(IsWalking, true);
            }
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