using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class ComponentModule : MonoBehaviour, IComponentModule
    {
        [SerializeField] private int _inputPortCount;
        [SerializeField] private int _outputPortCount;
        private List<IModulePort> _ports;

        public IEnumerable<IEdge> Edges => _ports.Select(x => x.Connection);
        public IEnumerable<IModulePort> Ports => _ports;

        private void Awake()
        {
            for (int i = 0; i < _inputPortCount; i++)
                _ports.Add(new ModulePort(this, PortDirection.Input));
            for (int i = 0; i < _outputPortCount; i++)
                _ports.Add(new ModulePort(this, PortDirection.Output));
        }

        T IComponentModule.GetComponent<T>()
        {
            return base.GetComponent<T>();
        }
    }
}