using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    [CreateAssetMenu(menuName = "Ports/Tag Port Initialization")]
    public class TagPortFactory : PortFactoryBase
    {
        [SerializeField] private List<PortTag>  _portInfos;
        
        public override IEnumerable<IDataPort<IExtensibleItem>> Create(IDataModule<IExtensibleItem> module)
        {
            List<IDataPort<IExtensibleItem> > ports = new();
            
            foreach (PortTag portInfo in _portInfos)
            {
                ITag tag = portInfo.Tag ?? new FreeTag();
                IDataPort<IExtensibleItem> port =portInfo.Direction == PortDirection.None ? new TaggedPort<IExtensibleItem>(module, tag) : new EnumChangingTaggedPort<PortDirection,IExtensibleItem>(module, tag, portInfo.Direction);
                ports.Add(port);
            }
            
            return ports;
        }
    }
}