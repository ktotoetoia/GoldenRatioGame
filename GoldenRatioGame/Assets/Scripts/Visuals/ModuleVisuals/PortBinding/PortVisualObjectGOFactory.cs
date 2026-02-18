using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "Ports/Port Visual Object GO Factory")]
    public class PortVisualObjectGOFactory : PortVisualObjectFactoryBase
    {
        [SerializeField] private GameObject _portVisualObjectMonoPrefab;
        
        public override void CreateVisualObjects(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects, IModuleVisualObject moduleVisualObject)
        {
            if (moduleVisualObject is not MonoBehaviour mb) throw new ArgumentException($"{nameof(PortVisualObjectGOFactory)} works only with MonoBehaviours");
            
            PortVisualObjectMono[] foundObjects = mb.GetComponentsInChildren<PortVisualObjectMono>();
            List<IPort> portsList = ports.ToList();

            for(int i = 0; i < portsList.Count; i++)
            {
                PortVisualObjectMono portVisualObject;

                if (foundObjects.Length > i)
                {
                    portVisualObject = foundObjects[i];
                }
                else
                {
                    portVisualObject = Instantiate(_portVisualObjectMonoPrefab).GetComponent<PortVisualObjectMono>();
                    portVisualObject.Transform.Transform.SetParent(moduleVisualObject.Transform.Transform,false);
                }
                
                portVisualObjects.Add(portVisualObject);
                portVisualObject.Initialize(moduleVisualObject, portsList[i]);
            }
        }
    }
}