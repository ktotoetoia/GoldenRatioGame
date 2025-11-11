using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthExtensionsObserver : IModuleGraphObserver
    {
        private readonly IFloatHealthValuesGroup _floatHealthValuesGroup;
        private readonly HashSet<IHealthExtension> _knownHealthModules = new();

        public HealthExtensionsObserver(IFloatHealthValuesGroup floatHealthValuesGroup)
        {
            _floatHealthValuesGroup = floatHealthValuesGroup ?? throw new ArgumentNullException(nameof(floatHealthValuesGroup));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            HashSet<IHealthExtension> currentHealthModules = new();

            foreach (IModule module in graph.Modules)
            {
                if (module is IGameModule gameModule && gameModule.Extensions.TryGetExtension(out IHealthExtension healthExt))
                {
                    currentHealthModules.Add(healthExt);
                }
            }

            foreach (IHealthExtension healthExt in currentHealthModules)
            {
                if (_knownHealthModules.Add(healthExt))
                {
                    _floatHealthValuesGroup.AddHealth(healthExt.Health);
                }
            }

            foreach (IHealthExtension healthExt in _knownHealthModules.Except(currentHealthModules).ToList())
            {
                _knownHealthModules.Remove(healthExt);
                _floatHealthValuesGroup.RemoveHealth(healthExt.Health);
            }
        }
    }
}