using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IHierarchyElement : INode
    {
        IHierarchyElement Parent { get; }
        IReadOnlyList<IHierarchyElement> Children { get; }
        
        void AddChild(IHierarchyElement child);
        bool RemoveChild(IHierarchyElement child);
        void SetParent(IHierarchyElement newParent);
        bool Contains(IHierarchyElement child);
        void AddChildInternal(IHierarchyElement child);
        bool RemoveChildInternal(IHierarchyElement child);
        void SetParentInternal(IHierarchyElement parent);
    }
}