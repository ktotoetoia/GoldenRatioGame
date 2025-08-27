using System.Collections.Generic;

namespace IM.Modules
{
    public interface IModule2
    {
        IEnumerable<IModuleConnector> Connectors { get; }
        
        bool CanConnect(IModule2 module,IModuleConnector connector);
        bool TryConnect(IModule2 module,IModuleConnector connector);
        bool CanDisconnect(IModuleConnector connector);
        bool Disconnect(IModuleConnector connector);
    }
}