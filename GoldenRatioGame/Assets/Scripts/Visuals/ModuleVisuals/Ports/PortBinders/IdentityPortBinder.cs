using System;
using System.Collections.Generic;
using System.Linq;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "Ports/Identity Port Binder")]
    public class IdentityPortBinder : PortBinderBase
    {
        public override void Bind(IList<IPortVisualObject> portVisualObjects)
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