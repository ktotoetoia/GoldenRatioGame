using System;
using System.Linq;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthExtensionsObserver : IModuleGraphSnapshotObserver
    {
        private readonly IFloatHealthValuesGroup _floatHealthValuesGroup;
        private readonly EnumerableDiffTracker<IHealthExtension> _diffTracker = new ();

        public HealthExtensionsObserver(IFloatHealthValuesGroup floatHealthValuesGroup)
        {
            _floatHealthValuesGroup = floatHealthValuesGroup ?? throw new ArgumentNullException(nameof(floatHealthValuesGroup));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            DiffResult<IHealthExtension> diffResult = _diffTracker.Update(graph.Modules.Where(x => x is IGameModule module && module.Extensions.HasExtensionOfType<IHealthExtension>())
                .SelectMany(x => (x as IGameModule).Extensions.GetExtensions<IHealthExtension>()));
            
            foreach (IHealthExtension healthExt in diffResult.Added)
            {
                _floatHealthValuesGroup.AddHealth(healthExt.Health);
            }
            
            foreach (IHealthExtension healthExt in diffResult.Removed)
            {
                _floatHealthValuesGroup.RemoveHealth(healthExt.Health);
            }
        }
    }
}