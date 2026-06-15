using System.Collections.Generic;

namespace IM.Visuals
{
    public class DirtyTracker<T> : IDirtyTracker<T>
    {
        private readonly HashSet<T> _dirtyObjects = new();

        public IReadOnlyCollection<T> DirtyObjects=> _dirtyObjects;
        public bool HasDirty => _dirtyObjects.Count != 0;
        
        public void MarkDirty(T obj)
        {
            if (obj != null)
                _dirtyObjects.Add(obj);
        }

        public void MarkClean(T obj)
        {
            if (obj != null)
                _dirtyObjects.Remove(obj);
        }

        public void Clear()
        {
            _dirtyObjects.Clear();
        }
    }
}