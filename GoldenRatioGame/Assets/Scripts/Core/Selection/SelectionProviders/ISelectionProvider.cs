using UnityEngine;

namespace IM.SelectionSystem
{
    public interface ISelectionProvider
    {
        ISelection<T> SelectAt<T>(Vector3 position) where T : class;
        ISelection<T> SelectWithin<T>(Bounds bounds)where T : class;
    }
}