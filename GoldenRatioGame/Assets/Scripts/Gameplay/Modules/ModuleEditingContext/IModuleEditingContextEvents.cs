using System;

namespace IM.Modules
{
    public interface IModuleEditingContextEvents
    {
        public event Action<IExtensibleItem> AddedToContext;
        public event Action<IExtensibleItem> RemovedFromContext;
    }
}