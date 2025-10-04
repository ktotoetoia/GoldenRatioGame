using System.Collections.Generic;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthExtensionsObserver
    {
        private readonly List<IHealthExtension> _modulesUsed = new();
        private readonly IFloatHealthValuesGroup _floatHealthValuesGroup;

        public HealthExtensionsObserver(IFloatHealthValuesGroup floatHealthValuesGroup)
        {
            _floatHealthValuesGroup = floatHealthValuesGroup;
        }
        
        public void Add(IModule module)
        {
            if (!TryGetHealthModule(module, out IHealthExtension healthModule) || _modulesUsed.Contains(healthModule))
            {
                return;
            }

            _floatHealthValuesGroup.AddHealth(healthModule.Health);
            _modulesUsed.Add(healthModule);
        }

        public void Remove(IModule module)
        {
            if (!TryGetHealthModule(module, out IHealthExtension healthModule) || !_modulesUsed.Contains(healthModule))
            {
                return;
            }
            
            _floatHealthValuesGroup.RemoveHealth(healthModule.Health);
            _modulesUsed.Remove(healthModule);
        }

        private bool TryGetHealthModule(IModule module, out IHealthExtension healthExtension)
        {
            healthExtension = null;
            return module is  IExtensibleModule componentModule && componentModule.TryGetExtension(out healthExtension);
        }
    }
}