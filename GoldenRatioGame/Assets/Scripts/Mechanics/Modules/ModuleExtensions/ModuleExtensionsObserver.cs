using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public class ModuleExtensionsObserver<TExtension> : IEditorObserver<IModuleGraphReadOnly>
    {
        private readonly ModuleGraphSnapshotDiffer _differ;

        public ModuleExtensionsObserver(Action<IExtensibleModule, TExtension> onExtensionAdded, Action<IExtensibleModule, TExtension> onExtensionRemoved)
        {
            _differ = new ModuleGraphSnapshotDiffer()
            {
                OnModuleAdded = x =>
                {
                    if (x is not IExtensibleModule extensibleModule ||
                        !extensibleModule.Extensions.TryGetAll(out IEnumerable<TExtension> extensions))
                        return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionAdded(extensibleModule, extension);
                    }
                },
                
                OnModuleRemoved = x =>
                {
                    if (x is not IExtensibleModule extensibleModule ||
                        !extensibleModule.Extensions.TryGetAll(out IEnumerable<TExtension> extensions))
                        return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionRemoved(extensibleModule, extension);
                    }
                },
            };
        }
        
        public void OnSnapshotChanged(IModuleGraphReadOnly graph) => _differ.OnSnapshotChanged(graph);
    }
}