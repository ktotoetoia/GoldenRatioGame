using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IVisualModule module);
        void AddAndConnect(IVisualModule module, IPort ownerPort, IPort targetPort);
        void RemoveModule(IVisualModule module);
        IConnection Connect(IPort output, IPort input);
        void Disconnect(IConnection connection);
    }
}