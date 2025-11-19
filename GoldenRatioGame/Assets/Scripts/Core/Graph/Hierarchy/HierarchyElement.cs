using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class HierarchyElement : IHierarchyElement
    {
        private readonly List<IHierarchyElement> _children = new();
        private readonly ITraversal _traversal = new BreadthFirstTraversal();

        public IHierarchyElement Parent { get; private set; }
        public IReadOnlyList<IHierarchyElement> Children => _children;

        public IEnumerable<IEdge> Edges
        {
            get
            {
                foreach (IHierarchyElement child in _children)
                    yield return new Edge(this, child);

                if (Parent != null)
                    yield return new Edge(Parent, this);
            }
        }

        public void AddChild(IHierarchyElement child)
        {
            if (child == null || child == this || _children.Contains(child))
                return;

            if (_traversal.HasPathTo(child, this))
                throw new InvalidOperationException("Hierarchy cycle detected.");

            child.Parent?.RemoveChildInternal(child);

            child.SetParentInternal(this);

            AddChildInternal(child);
        }

        public bool RemoveChild(IHierarchyElement child)
        {
            if (child == null)
                return false;

            if (RemoveChildInternal(child))
            {
                child.SetParentInternal(null);
                return true;
            }

            return false;
        }

        public void SetParent(IHierarchyElement newParent)
        {
            if (newParent == this)
                return;

            if (Parent == newParent)
                return;

            if (newParent != null && _traversal.HasPathTo(newParent, this))
                throw new InvalidOperationException("Hierarchy cycle detected.");

            Parent?.RemoveChildInternal(this);

            SetParentInternal(newParent);

            if (newParent != null && !newParent.Contains(this))
                newParent.AddChildInternal(this);
        }

        public bool Contains(IHierarchyElement child)
        {
            return _children.Contains(child);
        }

        public void AddChildInternal(IHierarchyElement child)
        {
            _children.Add(child);
            OnChildAdded(child);
        }

        public bool RemoveChildInternal(IHierarchyElement child)
        {
            bool removed = _children.Remove(child);

            if (removed) OnChildRemoved(child);

            return removed;
        }

        public void SetParentInternal(IHierarchyElement parent)
        {
            var oldParent = Parent;
            OnParentChanging(oldParent, parent);
            Parent = parent;
            OnParentSet(parent);
        }

        protected virtual void OnChildAdded(IHierarchyElement child) { }
        protected virtual void OnChildRemoved(IHierarchyElement child) { }
        protected virtual void OnParentChanging(IHierarchyElement oldParent, IHierarchyElement newParent) { }
        protected virtual void OnParentSet(IHierarchyElement parent) { }
    }
}