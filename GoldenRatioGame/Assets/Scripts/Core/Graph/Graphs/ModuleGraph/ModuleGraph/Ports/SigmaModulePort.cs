using System;

namespace IM.Graphs
{
    public class SigmaModulePort : IModulePort
    {
        private readonly Func<IModulePort,bool> _canConnect;
        private readonly Func<IConnection, bool> _canDisconnect;
        
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public PortDirection Direction { get; }

        public SigmaModulePort(IModule module, PortDirection direction,Func<IModulePort,bool> canConnect,Func<IConnection,bool> canDisconnect)
        {
            Module = module;
            Direction = direction;
            _canConnect = canConnect;
            _canDisconnect = canDisconnect;
        }

        public bool CanConnect(IModulePort other)
        {
            return !IsConnected && _canConnect(other);
        }

        public void Connect(IConnection connection)
        {
            Connection = connection;
        }

        public bool CanDisconnect()
        {
            return IsConnected && _canDisconnect(Connection);
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}