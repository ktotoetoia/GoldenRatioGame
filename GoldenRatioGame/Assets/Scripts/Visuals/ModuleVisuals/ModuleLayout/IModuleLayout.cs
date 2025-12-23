using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleLayout : IModuleExtension
    {
        IVisualModule CreateTemporaryVisualModule(IDictionary<IPort, IVisualPort> visualPortMap);
    }
}