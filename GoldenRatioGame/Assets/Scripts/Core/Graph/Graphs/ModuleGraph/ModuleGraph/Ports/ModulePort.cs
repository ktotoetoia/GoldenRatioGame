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

        public void Connect(IConnection connection)
        {
            Connection = connection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}