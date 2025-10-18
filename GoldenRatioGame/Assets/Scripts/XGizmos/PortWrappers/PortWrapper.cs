using IM.Graphs;

namespace IM.ModuleGraphGizmosDebug
{
    public class PortWrapper : IPortVisualWrapper
    {
        public IPort Port { get;  }
        public IVisual Visual { get; }

        public PortWrapper(IPort port, IVisual visual)
        {
            Port = port;
            Visual = visual;
        }
    }
}