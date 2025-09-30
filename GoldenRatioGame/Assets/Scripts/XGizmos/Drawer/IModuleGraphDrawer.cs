using System.Collections.Generic;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IModuleGraphDrawer
    {
        IEnumerable<IModuleVisualWrapper> Modules { get; }
    }
}