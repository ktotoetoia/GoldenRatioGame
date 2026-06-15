using System.Collections.Generic;

namespace IM.Visuals
{
    public interface IDirtyTracker<T>
    {
        IReadOnlyCollection<T> DirtyObjects { get; }
        bool HasDirty { get; }

        void MarkDirty(T obj);
        void MarkClean(T obj);
        void Clear();
    }
}