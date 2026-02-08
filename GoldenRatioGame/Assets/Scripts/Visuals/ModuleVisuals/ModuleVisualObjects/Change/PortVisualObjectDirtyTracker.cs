using System.Collections.Generic;

namespace IM.Visuals
{
    public class PortVisualObjectDirtyTracker : IPortVisualObjectDirtyTracker
    {
        private readonly HashSet<IPortVisualObject> _dirtyObjects = new();

        public IReadOnlyCollection<IPortVisualObject> DirtyObjects=> _dirtyObjects;
        public bool HasDirty => _dirtyObjects.Count != 0;
        
        public void MarkDirty(IPortVisualObject obj)
        {
            if (obj != null)
                _dirtyObjects.Add(obj);
        }

        public void MarkClean(IPortVisualObject obj)
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