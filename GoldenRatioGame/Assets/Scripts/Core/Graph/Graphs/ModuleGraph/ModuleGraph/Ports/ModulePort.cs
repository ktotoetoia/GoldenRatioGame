namespace IM.Graphs
{
    public class ModulePort : IModulePort
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;

        public ModulePort(IModule module)
        {
            Module = module;
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