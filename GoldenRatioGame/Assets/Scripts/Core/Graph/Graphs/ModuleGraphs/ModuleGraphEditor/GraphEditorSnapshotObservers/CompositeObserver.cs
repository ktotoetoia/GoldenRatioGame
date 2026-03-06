using System.Collections.Generic;

namespace IM.Graphs
{
    public class CompositeObserver : IEditorObserver<IModuleGraphReadOnly>
    {
        private readonly List<IEditorObserver<IModuleGraphReadOnly>> _observers;

        public CompositeObserver(IEnumerable<IEditorObserver<IModuleGraphReadOnly>> observers)
        {
            _observers = new(observers);
        }

        public void AddObserver(IEditorObserver<IModuleGraphReadOnly> observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IEditorObserver<IModuleGraphReadOnly> observer)
        {
            _observers.Remove(observer);
        }

        public void OnSnapshotChanged(IModuleGraphReadOnly graph)
        {
            foreach(IEditorObserver<IModuleGraphReadOnly> observer in _observers)
            {
                observer.OnSnapshotChanged(graph);
            }
        }
    }
}