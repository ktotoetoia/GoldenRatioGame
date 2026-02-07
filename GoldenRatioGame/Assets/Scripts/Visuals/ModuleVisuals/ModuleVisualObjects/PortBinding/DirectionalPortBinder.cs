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
        [SerializeField] private GameObject _portVisualObjectMonoPrefab;
        
        public override void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            List<IPort> portList = new List<IPort>(ports);
            if (portList.Count > _portPositionRotations.Count)
                throw new InvalidOperationException("There is more ports than port infos");
            
            for (int i = 0; i < _portPositionRotations.Count; i++)
            {
                IPort port =  portList[i];
                PortPositionRotation portInfo = _portPositionRotations[i];
                PortVisualObjectMono portVisualObject = Instantiate(_portVisualObjectMonoPrefab).GetComponent<PortVisualObjectMono>();
                
                LocalTransformReadOnly defaultTransform = new LocalTransformReadOnly(portInfo.Position,
                    Quaternion.Euler(0f, 0f, portInfo.EulerZRotation), Vector3.one);
                
                portVisualObjects.Add(portVisualObject);
                portVisualObject.Initialize(moduleVisualObject, port, portInfo.OutputOrderAdjustment, defaultTransform);
                portVisualObject.Transform.Transform.SetParent(moduleVisualObject.Transform.Transform,false);
            }
        }
    }
}