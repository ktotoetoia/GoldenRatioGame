using System;

namespace IM.Graphs
{
    public interface ICoreModuleGraphEvents : IModuleGraphEvents
    {
        event Action<IModule> OnCoreModuleSet;
    }
}