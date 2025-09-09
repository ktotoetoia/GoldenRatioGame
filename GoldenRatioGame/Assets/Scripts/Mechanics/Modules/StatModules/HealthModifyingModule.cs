using IM.Economy;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HealthModifyingModule : Module, IHealthModule
    {
        private readonly CappedValue<float> _health;
        
        public HealthModifyingModule(float maxHealth, float currentHealth)
        {
            _health = new CappedValue<float>(0,maxHealth,currentHealth);

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

        public ICappedValue<float> GetHealth()
        {
            return _health;
        }
    }
}