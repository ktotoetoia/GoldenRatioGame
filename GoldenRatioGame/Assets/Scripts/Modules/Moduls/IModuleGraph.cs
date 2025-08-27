using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleGraph : IGraphReadOnly<IModule>
    {
        public INode<IModule> Root { get; }
    }

    public class ModuleGraph : IModuleGraph
    {   
        private List<INode<IModule>> _nodes = new();
        
        public IReadOnlyList<INode<IModule>> Nodes => _nodes;
        public IReadOnlyList<IEdge<IModule>> Edges { get; }
        public INode<IModule> Root { get; }

        public ModuleGraph(IModule root)
        {
            
        }
        
        public IEdge<IModule> Connect(IModule first, IModule second)
        {
            throw new System.NotImplementedException();
            
        }

        public void Disconnect(IEdge<IModule> edge)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ModuleNode : INode<IModule>
    {
        public IModule Value { get; }
        public IEnumerable<IEdge<IModule>> Edges { get; }
    }
}