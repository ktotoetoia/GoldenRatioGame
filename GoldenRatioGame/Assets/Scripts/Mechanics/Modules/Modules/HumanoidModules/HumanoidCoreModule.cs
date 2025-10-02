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
            Extensions = new List<IModuleExtension> 
            {
                new HealthExtension(health) ,
                new SpeedExtension(new SpeedModifier(1f)),
            };
            
            AddPort(HeadPort = new FuncModulePort(this,PortDirection.Output,CanConnect,CanDisconnect));
            AddPort(LeftArmPort = new FuncModulePort(this,PortDirection.Output,CanConnect,CanDisconnect));
            AddPort(RightArmPort = new FuncModulePort(this,PortDirection.Output,CanConnect,CanDisconnect));
            AddPort(LeftLegPort = new FuncModulePort(this,PortDirection.Output,CanConnect,CanDisconnect));
            AddPort(RightLegPort = new FuncModulePort(this,PortDirection.Output,CanConnect,CanDisconnect));
        }

        private bool CanConnect(IModulePort modulePort)
        {
            return true;
        }

        private bool CanDisconnect(IConnection connection)
        {
            return false;
        }

        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}