using System.Collections.Generic;
using System.Linq;
using IM.Values;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HumanoidCoreModuleContext : IModuleContext
    {
        public IModuleContextExtensions Extensions { get; }
        
        public HumanoidCoreModuleContext(float maxHealth, float currentHealth) : this(new CappedValue<float>(0,maxHealth,currentHealth))
        {
            
        }

        public HumanoidCoreModuleContext(ICappedValue<float> health)
        {
            Extensions = new ModuleContextExtensions(new IModuleExtension[] 
            {
                new HealthExtension(health) ,
                new SpeedExtension(new SpeedModifier(1f)),
            });
        }

        public IModule Create()
        {
            ModuleContextWrapper module =  new ModuleContextWrapper(this);
            module.AddPort(new ModulePort(module,PortDirection.Output));
            module.AddPort(new ModulePort(module,PortDirection.Output));
            module.AddPort(new ModulePort(module,PortDirection.Output));
            module.AddPort(new ModulePort(module,PortDirection.Output));
            module.AddPort(new ModulePort(module,PortDirection.Output));

            return module;
        }
    }
}