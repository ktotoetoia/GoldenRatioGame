using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IPortVisualObjectDirtyTracker
    {
        IReadOnlyCollection<IPortVisualObject> DirtyObjects { get; }

        bool HasDirty { get; }
        void MarkDirty(IPortVisualObject obj);
        void MarkClean(IPortVisualObject obj);
        void Clear();
    }
}