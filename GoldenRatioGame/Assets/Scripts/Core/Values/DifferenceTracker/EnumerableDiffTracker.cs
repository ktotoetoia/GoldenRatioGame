using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Modules
{
    public sealed class EnumerableDiffTracker<T>
    {
        private readonly HashSet<T> _known;

        public EnumerableDiffTracker(IEqualityComparer<T> comparer = null)
        {
            _known = new HashSet<T>(comparer);
        }

        public DiffResult<T> Update(IEnumerable<T> current)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));

            HashSet<T> currentSet = new(current, _known.Comparer);

            HashSet<T> added = new();
            HashSet<T> removed = new();

            foreach (T item in currentSet)
            {
                if (_known.Add(item))
                {
                    added.Add(item);
                }
            }

            foreach (T item in _known.Except(currentSet).ToList())
            {
                _known.Remove(item);
                removed.Add(item);
            }

            return new DiffResult<T>(added, removed);
        }
    }
}