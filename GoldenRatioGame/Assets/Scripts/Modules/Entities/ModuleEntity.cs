using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        private ModuleGraphEvents _graph;
        
        public GameObject GameObject => gameObject;
        public IModuleGraph Graph => _graph;

        private void Awake()
        {
            _graph = new ModuleGraphEvents();
            _graph.OnGraphChanged += UpdateBuild;
        }
        
        private void UpdateBuild()
        {
            RemoveAllFromBuild();
            AddAllToBuild();
        }

        private void AddAllToBuild()
        {
            foreach (IGameModule module in Graph.Modules.OfType<IGameModule>())
            {
                module.AddToBuild(this);
            }
        }

        private void RemoveAllFromBuild()
        {
            foreach (IGameModule module in Graph.Modules.OfType<IGameModule>())
            {
                module.RemoveFromBuild(this);
            }
        }
    }
}