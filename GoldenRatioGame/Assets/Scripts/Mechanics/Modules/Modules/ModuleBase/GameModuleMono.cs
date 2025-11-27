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
        
        private void Awake()
        {
            Extensions = new ModuleExtensions(gameObject);
            
            Ports = new List<IPort>(GetComponent<IPortInitializer>().GetPorts());
        }
    }
}