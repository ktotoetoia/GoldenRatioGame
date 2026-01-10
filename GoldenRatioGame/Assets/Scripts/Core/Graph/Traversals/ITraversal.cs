using System.Collections.Generic;

namespace IM.Graphs
{
    public interface ITraversal
    {
        IEnumerable<TNode> Enumerate<TNode>(TNode start) where TNode : INode;
        IEnumerable<(TNode, TEdge)> EnumerateEdges<TNode,TEdge>(TNode start)  where TNode : INode where TEdge : IEdge;
        
        bool HasPath(INode from, INode to);

        IEnumerable<(TModule, TPort)> EnumerateModules<TModule, TPort>(TModule start) 
            where TModule : IModule 
            where TPort : IPort;

        public IEnumerable<(TModule, TPort )> EnumerateModulesAlongConnection<TModule, TPort>(TPort start)
            where TModule : class, IModule
            where TPort : class, IPort;
    }
} 