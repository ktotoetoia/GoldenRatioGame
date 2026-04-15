using System.Collections.Generic;

namespace IM.Graphs
{
    public class CompositeEditorObserver<TSnapshot> : IEditorObserver<TSnapshot>
    {
        private readonly IEnumerable<IEditorObserver<TSnapshot>> _observers;

        public CompositeEditorObserver(IEnumerable<IEditorObserver<TSnapshot>> observers)
        {
            _observers = observers;
        }

        public void OnSnapshotChanged(TSnapshot snapshot)
        {
            foreach (IEditorObserver<TSnapshot> observer in _observers)
            {
                observer.OnSnapshotChanged(snapshot);
            }
        }
    }
}