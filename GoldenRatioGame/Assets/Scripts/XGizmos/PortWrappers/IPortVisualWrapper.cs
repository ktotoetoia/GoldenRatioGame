using IM.Graphs;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IPortVisualWrapper
    {
        IPort Port { get;  }
        IVisual Visual { get;  }
    }
}