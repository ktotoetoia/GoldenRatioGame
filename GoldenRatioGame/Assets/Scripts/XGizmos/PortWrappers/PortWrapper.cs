using IM.Graphs;

namespace IM.ModuleGraphGizmosDebug
{
    public class PortWrapper : IPortVisualWrapper
    {
        public IModulePort Port { get;  }
        public IVisual Visual { get; }

        public PortWrapper(IModulePort port, IVisual visual)
        {
            Port = port;
            Visual = visual;
        }
    }
}