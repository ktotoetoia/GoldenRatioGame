using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class VisualModuleMono : MonoBehaviour, IVisualModule, IDisposable
    {
        private readonly List<IVisualPort> _ports = new();
        private Sprite _icon;
        private SpriteRenderer _renderer;
        
        public Sprite Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                _renderer ??= GetComponent<SpriteRenderer>();
                _renderer.sprite = _icon;
            }
        }
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IVisualPort> Ports => _ports;
        IEnumerable<IPort> IModule.Ports => _ports;
        public ITransform Transform { get; } = new Transform();
        
        private void Update()
        {
            transform.position = Transform.Position;
            transform.rotation = Transform.Rotation;
        }

        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}