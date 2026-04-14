using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public class ModuleExtensionsObserver<TExtension> : IEditorObserver<IDataModuleGraphReadOnly<IExtensibleItem>>
    {
        private readonly ModuleGraphSnapshotValueDiffer<IExtensibleItem> _differ;

        public ModuleExtensionsObserver(Action<IExtensibleItem, TExtension> onExtensionAdded, Action<IExtensibleItem, TExtension> onExtensionRemoved)
        {
            _differ = new ModuleGraphSnapshotValueDiffer<IExtensibleItem>()
            {
                ValueAdded= x =>
                {
                    if (!x.Extensions.TryGetAll(out IEnumerable<TExtension> extensions)) return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionAdded(x, extension);
                    }
                },
                
                ValueRemoved = x =>
                {
                    if (!x.Extensions.TryGetAll(out IEnumerable<TExtension> extensions)) return;
                    
                    foreach (TExtension extension in extensions)
                    {
                        onExtensionRemoved(x, extension);
                    }
                },
            };
        }
        
        public void OnSnapshotChanged(IDataModuleGraphReadOnly<IExtensibleItem> graph) => _differ.OnSnapshotChanged(graph);
    }
}