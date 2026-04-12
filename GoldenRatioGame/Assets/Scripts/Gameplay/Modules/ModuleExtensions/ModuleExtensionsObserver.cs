using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public class ModuleExtensionsObserver<TExtension> : IEditorObserver<IModuleGraphReadOnly>
    {
        private readonly ModuleGraphSnapshotDiffer _differ;

        public ModuleExtensionsObserver(Action<IExtensibleItem, TExtension> onExtensionAdded, Action<IExtensibleItem, TExtension> onExtensionRemoved)
        {
            _differ = new ModuleGraphSnapshotDiffer()
            {
                ModuleAdded = x =>
                {
                    if (x is not IDataModule<IExtensibleItem> extensibleModule ||
                        !extensibleModule.Value.Extensions.TryGetAll(out IEnumerable<TExtension> extensions))
                        return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionAdded(extensibleModule.Value, extension);
                    }
                },
                
                ModuleRemoved = x =>
                {
                    if (x is not IDataModule<IExtensibleItem> extensibleModule ||
                        !extensibleModule.Value.Extensions.TryGetAll(out IEnumerable<TExtension> extensions))
                        return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionRemoved(extensibleModule.Value, extension);
                    }
                },
            };
        }
        
        public void OnSnapshotChanged(IModuleGraphReadOnly graph) => _differ.OnSnapshotChanged(graph);
    }
}