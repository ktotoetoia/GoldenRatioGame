using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using IM.Economy;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public sealed class HumanoidCoreModule : Module, IEntityHolder
    {
        private readonly HealthHolder _healthHolder;

        public IEntity Entity
        {
            get => _healthHolder.Entity; 
            set
            {
                Debug.Log(value);
                _healthHolder.Entity = value;
            }
        }
        
        public IModulePort HeadPort { get; }
        public IModulePort LeftArmPort { get; }
        public IModulePort RightArmPort { get; }
        public IModulePort LeftLegPort { get; }
        public IModulePort RightLegPort { get; }
        
        public HumanoidCoreModule(float maxHealth, float currentHealth)
        {
            _healthHolder = new HealthHolder(maxHealth,currentHealth);

            AddPort(HeadPort = new ModulePort(this,PortDirection.Output));
            AddPort(LeftArmPort = new ModulePort(this,PortDirection.Output));
            AddPort(RightArmPort = new ModulePort(this,PortDirection.Output));
            AddPort(LeftLegPort = new ModulePort(this,PortDirection.Output));
            AddPort(RightLegPort = new ModulePort(this,PortDirection.Output));
        }
    }
}