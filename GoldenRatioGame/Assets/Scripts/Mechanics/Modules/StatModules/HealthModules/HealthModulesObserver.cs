using System.Collections.Generic;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthModulesObserver : IModuleObserver
    {
        private readonly List<IHealthModule> _modulesUsed = new();
        private readonly IFloatHealthValuesGroup _floatHealthValuesGroup;

        public HealthModulesObserver(IFloatHealthValuesGroup floatHealthValuesGroup)
        {
            _floatHealthValuesGroup = floatHealthValuesGroup;
        }
        
        public void Add(IModule module)
        {
            if (module is IComponentModule gameModule && gameModule.TryGetComponent(out IHealthModule g))
            {
                
            }
            
            if (module is not IHealthModule healthModule)
            {
                return;
            }
            
            _floatHealthValuesGroup.AddHealth(healthModule.Health);
            _modulesUsed.Add(healthModule);
        }

        public void Remove(IModule module)
        {
            if (module is not IHealthModule healthModule || !_modulesUsed.Contains(healthModule))
            {
                return;
            }
            
            _floatHealthValuesGroup.RemoveHealth(healthModule.Health);
            _modulesUsed.Remove(healthModule);
        }
    }
}