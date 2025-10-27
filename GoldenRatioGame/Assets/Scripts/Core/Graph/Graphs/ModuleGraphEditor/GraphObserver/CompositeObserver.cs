using System.Collections.Generic;

namespace IM.Graphs
{
    public class CompositeObserver : IModuleGraphObserver
    {
        private readonly List<IModuleGraphObserver> _observers;

        public CompositeObserver(IEnumerable<IModuleGraphObserver> observers)
        {
            _observers = new(observers);
        }

        public void AddObserver(IModuleGraphObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IModuleGraphObserver observer)
        {
            _observers.Remove(observer);
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            foreach(IModuleGraphObserver observer in _observers)
            {
                observer.OnGraphUpdated(graph);
            }
        }
    }
}