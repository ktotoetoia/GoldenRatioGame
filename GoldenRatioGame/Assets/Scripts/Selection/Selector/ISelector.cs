using System;
using UnityEngine;

namespace IM.SelectionSystem
{
    public interface ISelector : IHaveSelection
    {
        event Action<ISelection<object>> OnSelectionUpdated;
        ISelectionProvider SelectionProvider { get; }
        void UpdateSelectionAt(Vector3 position);
        void UpdateSelectionWithin(Bounds bounds);
    }
}