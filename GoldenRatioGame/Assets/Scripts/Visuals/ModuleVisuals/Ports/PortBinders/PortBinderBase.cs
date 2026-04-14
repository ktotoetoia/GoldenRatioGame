using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public abstract class PortBinderBase : ScriptableObject
    {
        public abstract void Bind(IList<IPortVisualObject> portVisualObjects);
        public abstract void GizmosPreview(Transform transform);
    }
}