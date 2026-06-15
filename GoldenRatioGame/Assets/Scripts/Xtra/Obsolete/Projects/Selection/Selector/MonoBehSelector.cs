using System;
using UnityEngine;

namespace IM.SelectionSystem
{
    public class MonoBehSelector : MonoBehaviour, ISelector
    {
        public event Action<ISelection<object>> OnSelectionUpdated;
        
        private ISelection<object> _selection;
        private ISelectionProvider _selectionProvider;

        public ISelectionProvider SelectionProvider
        {
            get => _selectionProvider ??= new RaycastSelectionProvider();
            set => _selectionProvider = value;
        }

        private void Awake()
        {
            _selection = new Selection<object>();
        }

        public void UpdateSelectionAt(Vector3 position)
        {
            _selection = SelectionProvider.SelectAt<object>(position);
            OnSelectionUpdated?.Invoke(_selection);    
        }

        public void UpdateSelectionWithin(Bounds bounds)
        {
            _selection = SelectionProvider.SelectWithin<object>(bounds);
            OnSelectionUpdated?.Invoke(_selection);
        }
        
        public ISelection<T> GetSelection<T>() where T : class
        {
            return _selection.OfType<T>();
        }
    }
}