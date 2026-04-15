using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public class CompositeObserverFactory<T> : IFactory<IEditorObserver<T>>
    {
        private readonly IEnumerable<IFactory<IEditorObserver<T>>> _observers;

        public CompositeObserverFactory(IEnumerable<IFactory<IEditorObserver<T>>> observers)
        {
            _observers = observers;
        }

        public IEditorObserver<T> Create()
        {
            return new CompositeEditorObserver<T>(_observers.Select(x => x.Create()).ToList());
        }
    }
}