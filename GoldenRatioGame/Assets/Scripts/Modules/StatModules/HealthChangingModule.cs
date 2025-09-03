using System.Linq;
using IM.Entities;
using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public sealed class HealthChangingModule : Module, IGameModule
    {
        private readonly IFloatHealth _health;
        
        public HealthChangingModule(float maxHealth, float currentHealth)
        {
            _health = new RawFloatHealth(0,maxHealth,currentHealth);

            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
        }
        
        public void AddToBuild(IEntity entity)
        {
            if (entity.GameObject.TryGetComponent(out IFloatHealthComposition component))
            {
                component.AddHealth(_health);
                
                return;
            }
            
            Debug.LogWarning("Entity does not contain required component");
        }

        public void RemoveFromBuild(IEntity entity)
        {            
            if (entity.GameObject.TryGetComponent(out IFloatHealthComposition component) && component.HealthComponents.Contains(_health))
            {
                component.RemoveHealth(_health);
            }
        }
    }
}