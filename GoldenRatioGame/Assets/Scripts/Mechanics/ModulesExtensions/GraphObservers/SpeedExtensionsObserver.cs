using System;
using IM.Graphs;
using IM.Values;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class SpeedExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private readonly EnumerableDiffTracker<ISpeedExtension> _diffTracker = new ();
        private IHaveSpeed _speed;

        private void Awake()
        {
            _speed = GetComponent<IHaveSpeed>();
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            DiffResult<ISpeedExtension> diffResult = _diffTracker.Update(graph.Modules.Where(x => x is IExtensibleModule module && module.Extensions.HasExtensionOfType<ISpeedExtension>())
                .SelectMany(x => (x as IExtensibleModule).Extensions.GetExtensions<ISpeedExtension>()));
            
            foreach (ISpeedExtension speedExt in diffResult.Added)
            {
                _speed.Speed.AddModifier(speedExt.SpeedModifier);
            }
            
            foreach (ISpeedExtension speedExt in diffResult.Removed)
            {
                _speed.Speed.RemoveModifier(speedExt.SpeedModifier);
            }
        }
    }
}