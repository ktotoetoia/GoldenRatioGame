using System;
using System.Collections.Generic;

namespace IM.Visuals
{
    public class Hierarchy<T> : IHierarchyElement<T> where T : class, IHierarchyElement<T>
    {
        private readonly T _owner;
        private T _parent;
        private readonly List<T> _children = new();

        public Hierarchy(T owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public T Parent => _parent;
        public IReadOnlyList<T> Children => _children.AsReadOnly();

        public void SetParent(T newParent, bool keepWorld = true)
        {
            _owner.SetParent(newParent, keepWorld);
        }

        public void AddChild(T child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.SetParent(_owner);
        }

        public bool RemoveChild(T child)
        {
            if (child == null) return false;
            if (child.Parent != _owner) return false;
            child.SetParent(null);
            return true;
        }

        public bool Contains(T child)
        {
            if (child == null) return false;
            return _children.Contains(child);
        }

        public void AddChildInternal(T child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (_children.Contains(child)) return;
            _children.Add(child);
        }

        public bool RemoveChildInternal(T child)
        {
            if (child == null) return false;
            return _children.Remove(child);
        }

        internal void SetParentInternal(T parent)
        {
            _parent = parent;
        }
    }
}