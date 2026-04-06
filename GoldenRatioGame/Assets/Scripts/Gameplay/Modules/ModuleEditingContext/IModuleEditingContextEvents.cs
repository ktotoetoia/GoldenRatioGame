using System;

namespace IM.Modules
{
    public interface IModuleEditingContextEvents
    {
        public event Action<IExtensibleModule> AddedToContext;
        public event Action<IExtensibleModule> RemovedFromContext;
    }
}