namespace IM.Graphs
{
    public class ModulePort : IModulePort
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public PortDirection Direction { get; }

        public ModulePort(IModule module, PortDirection direction)
        {
            Module = module;
            Direction = direction;
        }

        public bool CanConnect(IModulePort other)
        {
            return !IsConnected;
        }

        public virtual void Connect(IConnection connection)
        {
            Connection = connection;
        }

        public bool CanDisconnect()
        {
            return IsConnected;
        }

        public virtual void Disconnect()
        {
            Connection = null;
        }
    }
}