namespace IM.Graphs
{
    public interface IHierarchyElement : IHierarchyElementReadOnly
    {
        void AddChild(IHierarchyElement child);
        bool RemoveChild(IHierarchyElement child);
        void SetParent(IHierarchyElement newParent);
        void AddChildInternal(IHierarchyElement child);
        bool RemoveChildInternal(IHierarchyElement child);
        void SetParentInternal(IHierarchyElement parent);
    }
}