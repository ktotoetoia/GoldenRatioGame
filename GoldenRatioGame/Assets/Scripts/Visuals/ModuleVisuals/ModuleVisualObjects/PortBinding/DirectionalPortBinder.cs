using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public override void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            List<IPort> portList = new List<IPort>(ports);
            if (portList.Count() > _portPositionRotations.Count) throw new InvalidOperationException("There is more ports than port infos");

            for (int i = 0; i < portList.Count; i++)
            {
                IPort port =  portList[i];
                PortPositionRotation portInfo = _portPositionRotations[i];
                
                IHierarchyTransform portTransform = new HierarchyTransform
                {
                    LocalPosition = portInfo.Position,
                    LocalScale = Vector3.one,
                    LocalRotation = Quaternion.Euler(0f, 0f, portInfo.EulerZRotation)
                };
                
                portVisualObjects.Add(new PortVisualObject(moduleVisualObject, port, portTransform));
                moduleVisualObject.Transform.AddChildKeepLocal(portTransform);
            }
        }
    }
}