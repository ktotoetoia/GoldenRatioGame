using System.Collections.Generic;

namespace IM.Graphs
{
    public class CompositeObserver : IModuleGraphSnapshotObserver
    {
        private readonly List<IModuleGraphSnapshotObserver> _observers;

        public CompositeObserver(IEnumerable<IModuleGraphSnapshotObserver> observers)
        {
            _observers = new(observers);
        }

        public void AddObserver(IModuleGraphSnapshotObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IModuleGraphSnapshotObserver observer)
        {
            _observers.Remove(observer);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            foreach(IModuleGraphSnapshotObserver observer in _observers)
            {
                observer.OnGraphUpdated(graph);
            }
        }
    }
}