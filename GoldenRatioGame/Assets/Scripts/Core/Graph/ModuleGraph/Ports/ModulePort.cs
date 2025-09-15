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
        
        public virtual void Connect(IModuleConnection connection)
        {
            if (!IsConnected)
                Connection = connection;
        }

        public virtual void Disconnect()
        {
            if (IsConnected)
                Connection = null;
        }
    }
}