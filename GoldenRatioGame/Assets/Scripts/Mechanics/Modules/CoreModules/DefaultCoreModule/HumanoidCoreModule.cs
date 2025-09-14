using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using IM.Economy;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public sealed class HumanoidCoreModule : Module, ICoreModule, IHealthModule
    {
        private readonly IEntity _entity;
        private readonly CappedValue<float> _health;
        private readonly BreadthFirstTraversal _traversal = new();
        private readonly List<IModulesObserver> _graphObservers;
        
        public IModulePort HeadPort { get; }
        public IModulePort LeftArmPort { get; }
        public IModulePort RightArmPort { get; }
        public IModulePort LeftLegPort { get; }
        public IModulePort RightLegPort { get; }
        
        public HumanoidCoreModule(IEntity entity, float maxHealth, float currentHealth)
        {
            _entity = entity;
            _health = new CappedValue<float>(0,maxHealth,currentHealth);

            AddPort(HeadPort = new ModulePort(this,PortDirection.Output));
            AddPort(LeftArmPort = new ModulePort(this,PortDirection.Output));
            AddPort(RightArmPort = new ModulePort(this,PortDirection.Output));
            AddPort(LeftLegPort = new ModulePort(this,PortDirection.Output));
            AddPort(RightLegPort = new ModulePort(this,PortDirection.Output));
        
            _graphObservers = new List<IModulesObserver>(0)
            {
                new HealthModulesObserver(),
            };
        }
        
        public void OnStructureUpdated()
        {
            IGraphReadOnly subGraph = _traversal.GetSubGraph(this, x =>  true);

            foreach (IModulesObserver observer in _graphObservers)
            {
                observer.OnGraphStructureChanged(subGraph,_entity);
            }
        }

        public ICappedValue<float> GetHealth()
        {
            return _health;
        }
    }
}