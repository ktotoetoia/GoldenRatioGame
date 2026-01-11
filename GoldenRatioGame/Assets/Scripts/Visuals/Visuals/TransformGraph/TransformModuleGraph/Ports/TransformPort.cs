using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public class TransformPort : ITransformPort
    {
        public ITransformModule Module { get; }
        IModule IPort.Module => Module;
        public ITransformConnection Connection { get; private set; }
        IConnection IPort.Connection => Connection;
        public bool IsConnected => Connection != null;
        public IHierarchyTransform Transform { get; }

        public TransformPort(ITransformModule module) : this(module, new HierarchyTransform())
        {
            
        }

        public TransformPort(ITransformModule module, IHierarchyTransform transform)
        {
            Module = module;
            Transform = transform;
        }

        public void Connect(IConnection connection)
        {
            if(connection is not ITransformConnection visualConnection)
                throw new System.Exception("Invalid connection type");
            Connection = visualConnection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}