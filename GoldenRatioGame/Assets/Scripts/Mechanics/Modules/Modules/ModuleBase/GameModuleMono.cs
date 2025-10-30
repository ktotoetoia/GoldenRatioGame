using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        [SerializeField] private int _portCount;
        [SerializeField] private Sprite _sprite;
        private readonly List<IPort>  _ports = new();

        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IModuleExtensions Extensions { get; private set;}
        public IModuleLayout ModuleLayout { get; private set; }

        private void Awake()
        {
            for (int i = 0; i < _portCount; i++)
                _ports.Add(new Port(this));

            Extensions = GetComponent<IModuleExtensions>();
            ModuleLayout = new SquareModuleLayoutFactory().Create(this,_sprite);
        }
    }
}