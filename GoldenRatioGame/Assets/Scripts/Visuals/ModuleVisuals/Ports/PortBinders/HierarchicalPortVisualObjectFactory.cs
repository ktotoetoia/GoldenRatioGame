using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "Ports/Hierarchical Port Visual Object Factory")]
    public class HierarchicalPortVisualObjectFactory : PortVisualObjectFactoryBase
    {
        [SerializeField] private GameObject _portVisualObjectMonoPrefab;
        
        public override void CreateVisualObjects(IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            if (moduleVisualObject is not MonoBehaviour mb) throw new ArgumentException($"{nameof(HierarchicalPortVisualObjectFactory)} works only with MonoBehaviours");
            
            PortVisualObjectMono[] foundObjects = mb.GetComponentsInChildren<PortVisualObjectMono>();

            foreach (PortVisualObjectMono portVisualObject in foundObjects)
            {
                portVisualObject.Transform.Transform.SetParent(moduleVisualObject.Transform.Transform, false);
                portVisualObjects.Add(portVisualObject);
                portVisualObject.Initialize(moduleVisualObject);
            }
        }
    }
}