using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IHierarchyElementReadOnly : INode
    {
        IHierarchyElementReadOnly Parent { get; }
        IReadOnlyList<IHierarchyElementReadOnly> Children { get; }
        bool ContainsChild(IHierarchyElementReadOnly child);
    }
}