using System;
using UnityEngine;

namespace IM.SelectionSystem
{
    public class RaycastSelectionProvider : ISelectionProvider
    {
        public ISelection<T> SelectAt<T>(Vector3 position) where T : class
        {
            foreach (Collider2D collider in Physics2D.OverlapPointAll(position))          
            {
                if (collider.TryGetComponent(out ISelectable selectable) && selectable.TryGetObject(out T obj))
                {
                    return new Selection<T>(obj,position);
                }
            }
            
            return new Selection<T>();
        }

        public ISelection<T> SelectWithin<T>(Bounds bounds) where T : class
        {
            throw new NotImplementedException();
        }
    }
}