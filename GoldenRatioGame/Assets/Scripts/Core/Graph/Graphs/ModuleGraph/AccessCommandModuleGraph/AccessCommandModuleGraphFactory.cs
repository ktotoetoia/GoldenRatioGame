using IM.Base;

namespace IM.Graphs
{
    public class AccessCommandModuleGraphFactory : IFactory<AccessCommandModuleGraph, ICommandModuleGraph>
    {
        public AccessCommandModuleGraph Create(ICommandModuleGraph graph)
        {
            return new AccessCommandModuleGraph(graph) {ThrowIfCantUse = true, CanUse = true};
        }
    }
}