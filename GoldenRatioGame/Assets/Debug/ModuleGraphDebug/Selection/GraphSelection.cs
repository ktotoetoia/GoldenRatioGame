using System;
using IM.Graphs;
using IM.SelectionSystem;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class GraphSelection : ISelector
    {
        public event Action<ISelection<object>> OnSelectionUpdated;
        public ISelectionProvider SelectionProvider { get; }

        private ISelection<object> _current = new Selection<object>();

        public GraphSelection(IModuleGraphReadOnly graph)
        {
            SelectionProvider = new ModuleGraphSelectionProvider(graph);
        }

        public void UpdateSelectionAt(Vector3 position)
        {
            _current = SelectionProvider.SelectAt<object>(position);
            OnSelectionUpdated?.Invoke(_current);
        }

        public void UpdateSelectionWithin(Bounds bounds)
        {
            _current = SelectionProvider.SelectWithin<object>(bounds);
            OnSelectionUpdated?.Invoke(_current);
        }

        public ISelection<T> GetSelection<T>() where T : class
        {
            return _current.OfType<T>();
        }
    }
}