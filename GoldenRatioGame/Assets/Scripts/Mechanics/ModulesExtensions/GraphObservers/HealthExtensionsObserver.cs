using System;
using System.Linq;
using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public class HealthExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private readonly EnumerableDiffTracker<IHealthExtension> _diffTracker = new ();
        private IFloatHealthValuesGroup _floatHealthValuesGroup;

        private void Awake()
        {
            _floatHealthValuesGroup = GetComponent<IFloatHealthValuesGroup>();
        }
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            DiffResult<IHealthExtension> diffResult = _diffTracker.Update(graph.Modules.Where(x => x is IExtensibleModule module && module.Extensions.HasExtensionOfType<IHealthExtension>())
                .SelectMany(x => (x as IExtensibleModule).Extensions.GetExtensions<IHealthExtension>()));
            
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