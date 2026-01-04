namespace IM.Graphs
{
    public class Port : IPort
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;

        public Port(IModule module)
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