using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        [SerializeField] private Sprite _sprite;

        public IEnumerable<IEdge> Edges => Ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports { get; private set; }
        public IModuleExtensions Extensions { get; private set; }
        public IModuleLayout ModuleLayout { get; private set; }

        private void Awake()
        {
            IEnumerable<(IPort, IPortLayout)> created = GetComponent<IPortInitializer>().GetPorts(this);
            
            Extensions = new ModuleExtensions(gameObject);

            Ports = new List<IPort>(created.Select(x => x.Item1));
            ModuleLayout = new ModuleLayout(this,created.Select(x => x.Item2), _sprite, _sprite.bounds);
        }
    }
}