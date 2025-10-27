using System;
using System.Collections.Generic;
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
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            _coreGameModule = graph.Modules.OfType<ICoreGameModule>().FirstOrDefault();
            
            foreach ((IGameModule gameModule, IConnection connection) in _traversal.EnumerateEdges<IGameModule,IConnection>(_coreGameModule))
            {
                
            }
        }
    }
    
    public class ModuleUnit
    {
        private ICoreGameModule _coreModule;
        private List<ModuleVisual> _visuals = new();
        
        public ModuleUnit(ICoreGameModule coreModule)
        {
            _coreModule = coreModule;
        }
    }

    public class ModuleVisual
    {
        
    }
}