using System.Collections.Generic;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IModuleGraphDrawer
    {
        public List<ModuleVisual> Modules { get; }
    }
}