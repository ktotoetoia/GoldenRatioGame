using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilitiesObserver : IGraphObserver,IAbilitiesPool
    {
        private readonly List<IAbility> _abilities = new();
        public ICoreModuleGraph Graph { get; private set; }
        public IEnumerable<IAbility> Abilities => _abilities;

        public AbilitiesObserver(ICoreModuleGraph graph)
        {
            Graph = graph;
        }
        
        public void OnGraphChange()
        {
            IGraphReadOnly coreSubgraph = Graph.GetCoreSubgraph();

            foreach (IAbility ability in coreSubgraph.Nodes.OfType<IAbility>())
            {
                _abilities.Add(ability);
            }
        }

        private Vector2 GetDirection()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}