using System.Collections.Generic;

namespace IM.Modules
{
    public sealed class DiffResult<T>
    {
        public IReadOnlyCollection<T> Added { get; }
        public IReadOnlyCollection<T> Removed { get; }

        public DiffResult(
            IReadOnlyCollection<T> added,
            IReadOnlyCollection<T> removed)
        {
            Added = added;
            Removed = removed;
        }

        public bool HasChanges => Added.Count > 0 || Removed.Count > 0;
    }
}