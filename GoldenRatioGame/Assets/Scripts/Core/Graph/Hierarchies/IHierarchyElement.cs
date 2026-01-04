using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IHierarchyElement : IHierarchyElementReadOnly
    {
        new IHierarchyElement Parent { get; }
        new IReadOnlyList<IHierarchyElement> Children { get; }
        void AddChild(IHierarchyElement child);
        bool RemoveChild(IHierarchyElement child);
        void SetParent(IHierarchyElement newParent);
        void AddChildInternal(IHierarchyElement child);
        bool RemoveChildInternal(IHierarchyElement child);
        void SetParentInternal(IHierarchyElement parent);
    }
}