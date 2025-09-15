using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HealthModifierModule : Module, IEntityHolder
    {
        private readonly HealthHolder _healthHolder;
        
        public IEntity Entity {get => _healthHolder.Entity; set => _healthHolder.Entity = value; }
        
        public HealthModifierModule(float maxHealth, float currentHealth)
        {
            _healthHolder = new HealthHolder(maxHealth,currentHealth);

            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
            AddPort(new ModulePort(this,PortDirection.Input));
            AddPort(new ModulePort(this,PortDirection.Output));
        }
    }
}