using System.Collections.Generic;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Ports/Port Visual Object Factory")]
    public class PortVisualObjectFactory : ScriptableObject
    {
        [SerializeField] private GameObject _portVisualObjectMonoPrefab;
        
        public void CreateVisualObjects(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            foreach (IPort port in ports)
            {
                PortVisualObjectMono portVisualObject = Instantiate(_portVisualObjectMonoPrefab).GetComponent<PortVisualObjectMono>();
                
                portVisualObjects.Add(portVisualObject);
                portVisualObject.Initialize(moduleVisualObject, port);
                portVisualObject.Transform.Transform.SetParent(moduleVisualObject.Transform.Transform,false);
            }
        }
    }
}