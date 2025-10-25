using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class VisualModuleGraphObserver : MonoBehaviour, IModuleGraphObserver
    {
        private readonly List<IGameModule> _modules = new();
        
        private void Update()
        {
            foreach (IGameModule module in _modules)
            {
                
            }
        }
        
        public void Update(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            _modules.Clear();
            _modules.AddRange(graph.Modules.OfType<IGameModule>());
        }
    }
}