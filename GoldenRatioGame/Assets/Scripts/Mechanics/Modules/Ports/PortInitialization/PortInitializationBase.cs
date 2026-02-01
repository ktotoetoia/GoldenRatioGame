using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public abstract class PortInitializationBase : ScriptableObject
    {
        public abstract void Initialize(IList<IPort> ports,IDictionary<IPort, IPortInfo> portsDictionary, IModule module);
    }
}