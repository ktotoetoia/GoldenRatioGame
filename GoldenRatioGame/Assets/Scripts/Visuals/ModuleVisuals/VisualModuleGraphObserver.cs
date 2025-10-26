using System;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class VisualModuleGraphObserver : MonoBehaviour, IModuleGraphObserver
    {
        [SerializeField] private GameObject _visualPrefab;
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        private ICoreGameModule _coreGameModule;


        private void Update()
        {
            if (_coreGameModule == null) return;
            
            foreach ((IGameModule gameModule, IConnection connection) in _traversal.EnumerateEdges<IGameModule,IConnection>(_coreGameModule))
            {
                
            }
        }
        
        public void Update(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            _coreGameModule = graph.Modules.OfType<ICoreGameModule>().FirstOrDefault();
        }
    }
}