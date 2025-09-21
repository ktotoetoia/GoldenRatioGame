using System;

namespace IM.Graphs
{
    public interface IModuleGraphEvents
    {
        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IConnection> OnConnected;
        public event Action<IConnection> OnDisconnected;
        public event Action OnGraphChanged;
    }
}