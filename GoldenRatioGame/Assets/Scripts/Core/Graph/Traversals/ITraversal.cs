using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public interface ITraversal
    {
        IGraphReadOnly GetSubGraph(INode start);
        IGraphReadOnly GetSubGraph(INode node, Func<IReadOnlyList<INode>, bool> canPathTo);
        bool HasPathTo(INode from, INode to);
    }
} 