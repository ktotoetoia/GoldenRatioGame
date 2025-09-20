using System;

namespace IM.Graphs
{
    public interface IModuleGraphEvents
    {
        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IModuleConnection> OnConnected;
        public event Action<IModuleConnection> OnDisconnected;
        public event Action OnGraphChanged;
    }
}