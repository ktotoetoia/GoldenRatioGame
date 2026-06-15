using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.SelectionSystem
{
    public class Selection<T> : ISelection<T>
    {
        private readonly List<T> _selected;

        public T First => _selected.FirstOrDefault();
        public IEnumerable<T> Selected => _selected;
        public Vector3 SelectionPosition { get; }

        public Selection() : this(new List<T>())
        {
        }

        public Selection(T oneSelected, Vector3 selectionPosition = default) : this(new List<T> { oneSelected },selectionPosition)
        {
        }

        public Selection(IEnumerable<T> selected,Vector3 selectionPosition = default) : this(selected.ToList(),selectionPosition)
        {
        }

        public Selection(List<T> selected,Vector3 selectionPosition = default)
        {
            _selected = new List<T>(selected);
            SelectionPosition = selectionPosition;
        }

        public ISelection<TType> OfType<TType>()
        {
            return new Selection<TType>(_selected.OfType<TType>());
        }
    }
}