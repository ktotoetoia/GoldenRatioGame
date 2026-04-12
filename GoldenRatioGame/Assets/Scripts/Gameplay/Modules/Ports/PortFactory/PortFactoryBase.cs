using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public abstract class PortFactoryBase : ScriptableObject, IPortFactory
    {
        public abstract IEnumerable<IDataPort<IExtensibleItem>> Create(IDataModule<IExtensibleItem> param1);
    }
}