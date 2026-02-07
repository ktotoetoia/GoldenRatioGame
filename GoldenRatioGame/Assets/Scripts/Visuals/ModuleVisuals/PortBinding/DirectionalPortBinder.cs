using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Ports/Directional Port Binder")]
    public class DirectionalPortBinder : PortBinderBase
    {
        [SerializeField] private List<PortPositionRotation>  _portPositionRotations;
        
        public override void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects)
        {
            List<IPort> portList = new List<IPort>(ports);
            if (portList.Count > _portPositionRotations.Count)
                throw new InvalidOperationException("There is more ports than port infos");
            
            for (int i = 0; i < _portPositionRotations.Count; i++)
            {
                if(portVisualObjects[i] is not PortVisualObjectMono portVisualObject) continue;
                
                PortPositionRotation portInfo = _portPositionRotations[i];
                
                LocalTransformReadOnly defaultTransform = new LocalTransformReadOnly(portInfo.Position,
                    Quaternion.Euler(0f, 0f, portInfo.EulerZRotation), Vector3.one);
                
                portVisualObject.OutputOrderAdjustment = portInfo.OutputOrderAdjustment;
                portVisualObject.LocalTransformReadOnly = defaultTransform;
            }
        }
    }
}