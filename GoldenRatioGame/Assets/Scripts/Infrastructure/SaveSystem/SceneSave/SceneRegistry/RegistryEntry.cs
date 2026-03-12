using System;
using UnityEngine;

namespace IM.SaveSystem
{
    internal class RegistryEntry
    {
        public string Id;
        public WeakReference<GameObject> WeakGo;
        public IStateSerializable StateSerializer;

        public IStateSerializable GetSerializer()
        {
            return StateSerializer ??
                   (WeakGo != null && WeakGo.TryGetTarget(out GameObject go) ? go.GetComponent<IStateSerializable>() : null);
        }
    }
}