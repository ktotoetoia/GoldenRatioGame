using IM.Values;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HumanoidCoreModule : Module, IHealthModule
    {
        public ICappedValue<float> Health { get; }
        
        public IModulePort HeadPort { get; }
        public IModulePort LeftArmPort { get; }
        public IModulePort RightArmPort { get; }
        public IModulePort LeftLegPort { get; }
        public IModulePort RightLegPort { get; }
        
        public HumanoidCoreModule(float maxHealth, float currentHealth)
        {
            Health = new CappedValue<float>(0,maxHealth, currentHealth);

            AddPort(HeadPort = new LimitPort(this,PortDirection.Output));
            AddPort(LeftArmPort = new LimitPort(this,PortDirection.Output));
            AddPort(RightArmPort = new LimitPort(this,PortDirection.Output));
            AddPort(LeftLegPort = new LimitPort(this,PortDirection.Output));
            AddPort(RightLegPort = new LimitPort(this,PortDirection.Output));
        }
    }
}