using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public interface IModuleLayout : IModuleExtension
    {
        IVisualModule CreateTemporaryVisualModule(IDictionary<IPort, IVisualPort> visualPortMap);
    }
}