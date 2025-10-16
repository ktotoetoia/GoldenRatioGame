using System.Collections.Generic;
using IM.Values;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HumanoidCoreModule : Module, IGameModule
    {
        public IModuleLayout Layout { get; }
        public IModuleContextExtensions Extensions { get; }
        
        public HumanoidCoreModule(float maxHealth, float currentHealth) : this(new CappedValue<float>(0,maxHealth,currentHealth))
        {
            
        }

        public HumanoidCoreModule(ICappedValue<float> health)
        {
            Extensions = new ModuleContextExtensions(new IModuleExtension[] 
            {
                new HealthExtension(health) ,
                new SpeedExtension(new SpeedModifier(1f)),
            });
            
            AddPort(new ModulePort(this));
            AddPort(new ModulePort(this));
            AddPort(new ModulePort(this));
            AddPort(new ModulePort(this));
        }
    }
}