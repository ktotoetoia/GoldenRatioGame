using System;
using UnityEngine;

namespace IM.SelectionSystem
{
    public class Selector : ISelector
    {  
        public event Action<ISelection<object>> OnSelectionUpdated;
        private readonly ISelectionProvider _selectionProvider;
        private ISelection<object> _selection;

        public ISelectionProvider SelectionProvider => _selectionProvider;
        
        public Selector(ISelectionProvider selectionProvider)
        {
            _selectionProvider = selectionProvider;
            _selection = new Selection<object>();
        }

        public void UpdateSelectionAt(Vector3 position)
        {
            _selection = _selectionProvider.SelectAt<object>(position);
            OnSelectionUpdated?.Invoke(_selection);
        }

        public void UpdateSelectionWithin(Bounds bounds)
        {
            _selection = _selectionProvider.SelectWithin<object>(bounds);
            OnSelectionUpdated?.Invoke(_selection);
        }
        
        public ISelection<T> GetSelection<T>() where T : class
        {
            return _selection.OfType<T>();
        }
    }
}