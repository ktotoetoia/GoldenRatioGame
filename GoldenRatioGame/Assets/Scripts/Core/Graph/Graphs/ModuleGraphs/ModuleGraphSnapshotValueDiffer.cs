using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class ModuleGraphSnapshotValueDiffer<T> : IEditorObserver<IDataModuleGraphReadOnly<T>>
    {
        private readonly List<T> _prevValues = new();
        
        public Action<T> ValueAdded { get; set; } = x => { };        
        public Action<T> ValueRemoved { get; set; } = x => { };
        
        public void OnSnapshotChanged(IDataModuleGraphReadOnly<T> graph)
        {
            foreach (T value in _prevValues.Except(graph.DataModules.Select(x => x.Value)))
            {
                ValueRemoved(value);
            }

            foreach (T value in graph.DataModules.Select(x => x.Value).Except(_prevValues))
            {
                ValueAdded(value);
            }
            
            _prevValues.Clear();
            _prevValues.AddRange(graph.DataModules.Select(x => x.Value));
        }
    }
}