using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentExtensionObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private AugmentProgressManager _progressManager;
    
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            _progressManager.SetActiveExtensions(GetExtensions(snapshot));
        }
    
        private static IEnumerable<IAugmentExtension> GetExtensions(IModuleEditingContextReadOnly snapshot)
        {
            return snapshot.Graph.DataModules
                .Where(x => x.Value.Extensions.HasOfType<IAugmentExtension>())
                .Select(x => x.Value.Extensions.Get<IAugmentExtension>());
        }
    }
}