using IM.Economy;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HealthModifierModule : Module, IHealthModule
    {
        public ICappedValue<float> Health { get; }
        
        public HealthModifierModule(float maxHealth, float currentHealth)
        {
            Health = new CappedValue<float>(0,maxHealth, currentHealth);
            
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