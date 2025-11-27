using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IHierarchyElementReadOnly : INode
    {
        IHierarchyElement Parent { get; }
        IReadOnlyList<IHierarchyElement> Children { get; }
        bool ContainsChild(IHierarchyElement child);
    }
}