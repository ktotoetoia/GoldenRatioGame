using IM.Graphs;

namespace IM.Visuals
{
    public class VisualPort : IVisualPort
    {
        public IVisualModule Module { get; }
        IModule IPort.Module => Module;
        public IVisualConnection Connection { get; private set; }
        IConnection IPort.Connection => Connection;
        public bool IsConnected => Connection != null;
        public IHierarchyTransform Transform { get; }

        public VisualPort(IVisualModule module) : this(module, new HierarchyTransform())
        {
            
        }

        public VisualPort(IVisualModule module, IHierarchyTransform transform)
        {
            Module = module;
            Transform = transform;
        }

        public void Connect(IConnection connection)
        {
            if(connection is not IVisualConnection visualConnection)
                throw new System.Exception("Invalid connection type");
            
            Connection = visualConnection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}