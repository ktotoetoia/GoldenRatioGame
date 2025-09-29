namespace IM.Graphs
{
    public class LimitPort : ModulePort, ILimitPort
    {
        public LimitPort(IModule module, PortDirection direction) : base(module, direction)
        {

        }


        public override void Connect(IConnection connection)
        {
            base.Connect(connection);
        }

        public override void Disconnect()
        {
            base.Disconnect();
        }

        public bool CanConnect(IModulePort modulePort)
        {
            return !IsConnected;
        }

        public bool CanDisconnect()
        {
            return IsConnected;
        }
    }
}