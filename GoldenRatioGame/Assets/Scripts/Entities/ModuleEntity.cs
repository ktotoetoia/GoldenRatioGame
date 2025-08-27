using IM.Graphs;
using UnityEngine;

namespace IM.Entities
{
    public class ModuleEntity : MonoBehaviour, IEntity
    {
        public GameObject GameObject => gameObject;
        public ModuleGraph Graph { get; private set; }
        
        private void Awake()
        {
            Graph = new ModuleGraph();
        }
    }
}