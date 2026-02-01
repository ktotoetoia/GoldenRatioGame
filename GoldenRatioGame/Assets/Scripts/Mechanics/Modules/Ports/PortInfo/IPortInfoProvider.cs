using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IPortInfoProvider
    {
        public IReadOnlyDictionary<IPort, IPortInfo> PortsInfos { get; }
    }
}