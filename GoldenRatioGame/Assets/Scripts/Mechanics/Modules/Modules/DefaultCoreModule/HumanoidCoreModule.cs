using System.Collections.Generic;
using System.Linq;
using IM.Values;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HumanoidCoreModule : Module, IExtensibleModule
    {
        public IModulePort HeadPort { get; }
        public IModulePort LeftArmPort { get; }
        public IModulePort RightArmPort { get; }
        public IModulePort LeftLegPort { get; }
        public IModulePort RightLegPort { get; }

        public IReadOnlyList<IModuleExtension> Extensions { get; }
        
        public HumanoidCoreModule(float maxHealth, float currentHealth) : this(new CappedValue<float>(0,maxHealth,currentHealth))
        {
            
        }

        public HumanoidCoreModule(ICappedValue<float> health)
        {
            Extensions = new List<IModuleExtension> { new HealthExtension(health) };
            
            AddPort(HeadPort = new LimitPort(this,PortDirection.Output));
            AddPort(LeftArmPort = new LimitPort(this,PortDirection.Output));
            AddPort(RightArmPort = new LimitPort(this,PortDirection.Output));
            AddPort(LeftLegPort = new LimitPort(this,PortDirection.Output));
            AddPort(RightLegPort = new LimitPort(this,PortDirection.Output));
        }

        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}