using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "Ports/Port Visual Object Factory")]
    public class PortVisualObjectFactory : PortVisualObjectFactoryBase
    {
        [SerializeField] private GameObject _portVisualObjectMonoPrefab;
        [SerializeField] private int _portCount = 1;
        
        public override void CreateVisualObjects(IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            for (int i = 0; i < _portCount; i++)
            {
                PortVisualObjectMono portVisualObject = Instantiate(_portVisualObjectMonoPrefab).GetComponent<PortVisualObjectMono>();
                
                portVisualObjects.Add(portVisualObject);
                portVisualObject.Initialize(moduleVisualObject);
                portVisualObject.Transform.Transform.SetParent(moduleVisualObject.Transform.Transform,false);
            }
        }
    }
}