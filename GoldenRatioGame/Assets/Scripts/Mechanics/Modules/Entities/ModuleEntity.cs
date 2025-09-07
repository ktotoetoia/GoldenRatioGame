using System.Collections.Generic;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private ModuleGraphEvents _graph;
        private List<IGraphReader> _graphReaders;
        
        public GameObject GameObject => gameObject;
        public IModuleGraph Graph => _graph;

        private void Awake()
        {
            _graph = new ModuleGraphEvents();
            _graph.OnGraphChanged +=() => _graphReaders.ForEach(x => x.OnGraphStructureChanged());
        
            _graphReaders = new List<IGraphReader>(0)
            {
                new EntityModuleGraphReader(_graph,this),
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _graph.AddModule(new HealthModifyingModule(_maxHealth,_maxHealth));
            }
        }
    }
}