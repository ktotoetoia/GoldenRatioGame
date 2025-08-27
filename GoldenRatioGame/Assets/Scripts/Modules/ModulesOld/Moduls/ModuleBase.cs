using System.Collections.Generic;

namespace IM.Modules
{
    public class ModuleBase : IModule2
    {
        protected readonly List<ModuleConnector>  _connectors = new();
        
        public IEnumerable<IModuleConnector> Connectors => _connectors;

        public ModuleBase()
        {
            _connectors.Add(new ModuleConnector(this));
        }
        
        public virtual bool CanConnect(IModule2 module, IModuleConnector connector)
        {
            return connector is ModuleConnector moduleConnector && _connectors.Contains(moduleConnector) && moduleConnector.To == null;
        }

        public virtual bool CanDisconnect(IModuleConnector connector)
        {
            return true;
        }

        public virtual bool TryConnect(IModule2 module, IModuleConnector connector)
        {
            if (CanConnect(module, connector))
            {
                (connector as ModuleConnector).To= module;

                return true;
            }

            return false;
        }

        public virtual bool Disconnect(IModuleConnector connector)
        {
            if (CanDisconnect(connector))
            {
                (connector as ModuleConnector).To = null;

                return true;
            }

            return false;
        }
    }
}