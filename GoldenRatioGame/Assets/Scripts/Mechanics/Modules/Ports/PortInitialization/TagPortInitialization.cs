using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Ports/Tag Port Initialization")]
    public class TagPortInitialization : PortInitializationBase
    {
        [SerializeField] private List<PortTag>  _portInfos;
        
        public override void Initialize(IList<IPort> ports,  IModule module)
        {
            if(ports.Count != 0) throw new ArgumentException("Ports must be empty to initialize");
            
            foreach (PortTag portInfo in _portInfos)
            {
                ITag tag = portInfo.Tag ?? new FreeTag();
                IPort port = new TaggedPort(module, tag);
                
                ports.Add(port);
            }
        }
    }
}