namespace IM.Graphs
{
    public class ModulePort : IModulePort
    {
        public IModule Module { get; }
        public IModuleConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public PortDirection Direction { get; }

        public ModulePort(IModule module, PortDirection direction)
        {
            Module = module;
            Direction = direction;
        }

        public virtual bool CanConnect(IModuleConnection connection) => !IsConnected;

        public virtual void Connect(IModuleConnection connection)
        {
            if (CanConnect(connection))
                Connection = connection;
        }

        public virtual bool CanDisconnect() => IsConnected;

        public virtual void Disconnect()
        {
            if (CanDisconnect())
                Connection = null;
        }
    }
}