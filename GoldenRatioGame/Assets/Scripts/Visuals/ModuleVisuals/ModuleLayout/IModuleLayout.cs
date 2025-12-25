using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleLayout : IModuleExtension
    {
        IVisualModule CreateVisualModule(IDictionary<IPort, IVisualPort> visualPortMap);
    }
}