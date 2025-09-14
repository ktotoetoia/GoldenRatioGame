using System.Collections.Generic;
using System.Linq;
using IM.Economy;
using IM.Entities;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthModulesObserver: IModulesObserver
    {
        public void OnGraphStructureChanged(IGraphReadOnly graph, IEntity entity)
        {
            if (!entity.GameObject.TryGetComponent(out IFloatHealthValuesGroup floatHealthValuesGroup))
            {
                 return;   
            }
            
            foreach (IHealthModule module in graph.Nodes.OfType<IHealthModule>())
            {
                ICappedValue<float> health = module.GetHealth();

                if (!floatHealthValuesGroup.Contains(health))
                {
                    floatHealthValuesGroup.AddHealth(health);
                }
            }
        }
    }
}