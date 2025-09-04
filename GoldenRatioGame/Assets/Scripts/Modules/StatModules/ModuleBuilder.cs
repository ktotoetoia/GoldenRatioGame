using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleBuilder :MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        private ModuleGraph _graph;
        private IEntity _entity;

        private void Awake()
        {
            _entity = GetComponent<IEntity>();
            _graph = new ModuleGraph();
            _graph.AddModule(new HealthModifyingModule(_maxHealth,_maxHealth));
            
            UpdateBuild();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _graph.AddModule(new HealthModifyingModule(_maxHealth,_maxHealth));
                UpdateBuild();
            }
        }

        private void UpdateBuild()
        {
            RemoveAllFromBuild();
            AddAllToBuild();
        }

        private void AddAllToBuild()
        {
            foreach (IGameModule module in _graph.Modules.OfType<IGameModule>())
            {
                module.AddToBuild(_entity);
            }
        }

        private void RemoveAllFromBuild()
        {
            foreach (IGameModule module in _graph.Modules.OfType<IGameModule>())
            {
                module.RemoveFromBuild(_entity);
            }
        }
    }
}