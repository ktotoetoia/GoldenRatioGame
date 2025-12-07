using System.Collections.Generic;
using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleLayoutToVisualModuleMonoConvertor : IFactory<IVisualModule, IModuleLayout, IDictionary<IPort, IVisualPort>>
    {
        private readonly GameObject _prefab;
        public Transform Parent { get; set; }

        public ModuleLayoutToVisualModuleMonoConvertor(GameObject prefab, Transform parent = null)
        {
            Parent = parent;
            _prefab = prefab;
        } 
        
        public IVisualModule Create(IModuleLayout moduleLayout, IDictionary<IPort, IVisualPort> dictionary)
        {
            
            VisualModuleMono visualModule = Parent == null ?
                Object.Instantiate(_prefab).GetComponent<VisualModuleMono>() :
                Object.Instantiate(_prefab,Parent).GetComponent<VisualModuleMono>();
            visualModule.Icon = moduleLayout.Icon;
            visualModule.HierarchyTransform.LocalScale = moduleLayout.Bounds.size;

            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(visualModule);
                
                visualModule.HierarchyTransform.AddChild(visualPort.Transform);

                visualPort.Transform.LocalPosition = portLayout.RelativePosition;
                visualPort.Transform.LocalScale = Vector3.one;
                
                
                visualPort.Transform.LocalRotation = Quaternion.LookRotation(portLayout.Normal, Vector3.up);
                
                visualModule.AddPort(visualPort);
                dictionary[portLayout.Port] = visualPort;
            }

            return visualModule;
        }
    }
}