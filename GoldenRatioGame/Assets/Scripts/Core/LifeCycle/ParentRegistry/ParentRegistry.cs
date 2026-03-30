using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    public class ParentRegistry : MonoBehaviour, IParentRegistry
    {
        private readonly HashSet<IParentRestorable> _trackedObjects = new();

        public IEnumerable<IParentRestorable> TrackedObjects => _trackedObjects;

        public void Register(IParentRestorable target) => _trackedObjects.Add(target);
        public void Unregister(IParentRestorable target) => _trackedObjects.Remove(target);
    }
}