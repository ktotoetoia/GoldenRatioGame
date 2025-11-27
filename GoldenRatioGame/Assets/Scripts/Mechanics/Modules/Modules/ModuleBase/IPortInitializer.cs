using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface IPortInitializer
    {
        IEnumerable<IPort> GetPorts();
    }
}