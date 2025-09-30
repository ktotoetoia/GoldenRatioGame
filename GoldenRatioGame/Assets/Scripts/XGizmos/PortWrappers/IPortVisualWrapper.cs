using IM.Graphs;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IPortVisualWrapper
    {
        IModulePort Port { get;  }
        IVisual Visual { get;  }
    }
}