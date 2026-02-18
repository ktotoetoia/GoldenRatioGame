using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "Ports/Empty Port Binder")]
    public class EmptyPortBinding : PortBinderBase
    {
        public override void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects)
        {
            foreach (PortVisualObjectMono portVisualObject in portVisualObjects.OfType<PortVisualObjectMono>())
            {
                portVisualObject.LocalTransformPreset = new LocalTransformPreset(portVisualObject.Transform.LocalPosition,portVisualObject.Transform.LocalRotation, portVisualObject.Transform.LocalScale);
            }
        }

        public override void GizmosPreview(Transform transform)
        {
            throw new NotImplementedException();
        }
    }
}