using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisualObserver : MonoBehaviour, IModuleGraphObserver
    {
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            GetComponent<IModuleGraphVisual>().Source = graph;
        }
    }
}