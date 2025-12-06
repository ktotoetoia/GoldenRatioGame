using System.Collections.Generic;
using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleLayoutToVisualModuleMonoConvertor : IFactory<IVisualModule, IModuleLayout, IDictionary<IPort, IVisualPort>>
    {
        private readonly GameObject _prefab;
        
        public ModuleLayoutToVisualModuleMonoConvertor(GameObject prefab)
        {
            _prefab = prefab;
        }
        
        public IVisualModule Create(IModuleLayout moduleLayout, IDictionary<IPort, IVisualPort> dictionary)
        {
            VisualModuleMono visualModule = Object.Instantiate(_prefab).GetComponent<VisualModuleMono>();
            visualModule.Icon = moduleLayout.Icon;
            visualModule.Transform.LocalScale = moduleLayout.Bounds.size;

            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(visualModule);
                
                visualModule.Transform.AddChild(visualPort.Transform);

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