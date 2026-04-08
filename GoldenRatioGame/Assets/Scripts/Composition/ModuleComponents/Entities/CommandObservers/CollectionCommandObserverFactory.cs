using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public class CollectionCommandObserverFactory<T> : ICommandObserverAddFactory,
        ICommandObserverRemoveFactory
    {
        private readonly Func<IModule, ICollection<IModule>, T> _getCollectionItemAdd;
        private readonly Func<IModule, ICollection<IModule>,  ICollection<IConnection>,T> _getCollectionItemRemove;
        private readonly ICollection<T> _source;

        public CollectionCommandObserverFactory(Func<IModule, ICollection<IModule>, T> getCollectionItemAdd, Func<IModule, ICollection<IModule>,  ICollection<IConnection>,T> getCollectionItemRemove, ICollection<T> source)
        {
            _getCollectionItemAdd = getCollectionItemAdd;
            _getCollectionItemRemove = getCollectionItemRemove;
            _source = source;
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2)
        {
            T itemValue = _getCollectionItemAdd(param1, param2);
            if (itemValue == null) return new EmptyCommandObserver();

            return new CollectionCommandObserver<T>(_source, itemValue);
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            T itemValue = _getCollectionItemRemove(param1, param2,param3);
            if (itemValue == null) return new EmptyCommandObserver();

            return new CollectionCommandObserver<T>(_source, itemValue);
        }
    }
}