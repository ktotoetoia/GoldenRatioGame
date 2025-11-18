using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IHierarchyElement<T>
    {
        T Parent { get; }
        IReadOnlyList<T> Children { get; }
        
        void SetParent(T newParent, bool keepWorld = true);
        void AddChild(T child);
        bool RemoveChild(T child);
        bool Contains(T child);
        void AddChildInternal(T child);
        bool RemoveChildInternal(T child);
    }
}