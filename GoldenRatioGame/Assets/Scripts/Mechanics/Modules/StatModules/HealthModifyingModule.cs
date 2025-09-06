using System.Linq;
using IM.Economy;
using IM.Entities;
using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public sealed class HealthModifyingModule : Module, IGameModule
    {
        private readonly CappedValue<float> _health;
        
        public HealthModifyingModule(float maxHealth, float currentHealth)
        {
            _health = new CappedValue<float>(0,maxHealth,currentHealth);

            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
        }
        
        public void AddToBuild(IEntity entity)
        {
            if (!entity.GameObject.TryGetComponent(out IFloatHealthValuesGroup component))
            {
                Debug.LogWarning("Entity does not contain required component");
             
                return;
            }

            component.AddHealth(_health);
        }
        
        public void RemoveFromBuild(IEntity entity)
        {            
            if (entity.GameObject.TryGetComponent(out IFloatHealthValuesGroup component) && component.Values.Contains(_health))
            {
                component.RemoveHealth(_health);
            }
        }
    }
}