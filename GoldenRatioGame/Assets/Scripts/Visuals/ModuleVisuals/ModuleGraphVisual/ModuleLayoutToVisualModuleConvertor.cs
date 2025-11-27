using System.Collections.Generic;
using IM.Base;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleLayoutToVisualModuleConvertor : IFactory<IVisualModule, IModuleLayout, IDictionary<IPort, IVisualPort>>
    {
        public IVisualModule Create(IModuleLayout moduleLayout, IDictionary<IPort, IVisualPort> dictionary)
        {
            VisualModule visualModule = new (moduleLayout.Icon)
            {
                Transform =
                {
                    Scale = moduleLayout.Bounds.size,
                }
            };

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