using System.Collections.Generic;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private CoreModuleGraph _graph;
        private List<IGraphReader> _graphReaders;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;

        private void Awake()
        {
            ModuleGraphEvents graphEvents = new ModuleGraphEvents(new SafeModuleGraph());
            
            _graph = new CoreModuleGraph(new HealthModifyingModule(_maxHealth,_maxHealth),graphEvents);

            graphEvents.OnGraphChanged +=UpdateGraphReaders;
        
            _graphReaders = new List<IGraphReader>(0)
            {
                new EntityModuleGraphReader(_graph,this),
            };

            UpdateGraphReaders();
        }

        private void UpdateGraphReaders()
        {
            _graphReaders.ForEach(x => x.OnGraphStructureChanged());
        }
    }
}