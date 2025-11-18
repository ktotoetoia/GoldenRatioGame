using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public interface IVisualModuleGraph : IModuleGraphReadOnly
    {
        ITransform Transform { get; }
        
        new IEnumerable<IVisualModule> Modules { get; }
        new IEnumerable<IVisualConnection> Connections { get; }
        void AddModule(IVisualModule module);
        void AddAndConnect(IVisualModule module, IVisualPort ownerPort, IVisualPort targetPort);
        void RemoveModule(IVisualModule module);
        IVisualConnection Connect(IVisualPort output, IVisualPort input);
        void Disconnect(IVisualConnection connection);
    }
}