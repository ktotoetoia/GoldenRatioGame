using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public interface ITraversal
    {
        IGraphReadOnly GetSubGraph(INode node, Func<IReadOnlyList<INode>, bool> canPathTo);
    }
} 