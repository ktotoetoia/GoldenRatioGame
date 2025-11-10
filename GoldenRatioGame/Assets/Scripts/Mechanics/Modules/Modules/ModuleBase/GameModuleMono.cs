using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        private List<Vector4> _portsInfosS;
        [SerializeField] private Sprite _sprite;
        private readonly List<IPort>  _ports = new();

        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IModuleExtensions Extensions { get; private set;}
        public IModuleLayout ModuleLayout { get; private set; }

        private void Awake()
        {
            Dictionary<IPort, Vector4> created = new();
            
            foreach (Vector4 portInfo in _portsInfosS)
            {
                IPort port = new Port(this);
                
                created.Add(port, portInfo);
                _ports.Add(port);
            }

            Extensions = new ModuleExtensions(gameObject);
            ModuleLayout = new ModuleLayout(this, created.Select(x =>new PortLayout(x.Key,new Vector3(x.Value.x,x.Value.y),new Vector3(x.Value.z, x.Value.w))),_sprite);
        }
    }
}