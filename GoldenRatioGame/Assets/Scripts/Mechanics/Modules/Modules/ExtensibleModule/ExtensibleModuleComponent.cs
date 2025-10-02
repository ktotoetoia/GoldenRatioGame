using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ExtensibleModuleComponent : MonoBehaviour, IExtensibleModule
    {
        [SerializeField] private int _inputPortCount;
        [SerializeField] private int _outputPortCount;
        private readonly List<IModulePort> _ports = new();
        private readonly List<IModuleExtension> _extensions = new();
        
        public IReadOnlyList<IModuleExtension> Extensions => _extensions;
        public IEnumerable<IEdge> Edges => _ports.Select(x => x.Connection);
        public IEnumerable<IModulePort> Ports => _ports;

        private void Awake()
        {
            GetComponents(_extensions);
            
            for (int i = 0; i < _inputPortCount; i++)
                _ports.Add(new ModulePort(this, PortDirection.Input));
            for (int i = 0; i < _outputPortCount; i++)
                _ports.Add(new ModulePort(this, PortDirection.Output));
        }

        public T GetExtension<T>()
        {
            return _extensions.OfType<T>().FirstOrDefault();
        }
    }
}