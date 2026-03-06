using System.Collections.Generic;
using System.Linq;

namespace IM.Modules
{
    public class TypeRegistry<TType> : ITypeRegistry<TType> where TType : class
    {
        private readonly HashSet<TType> _collection;
        
        public IReadOnlyCollection<TType> Collection => _collection;

        public TypeRegistry() : this(new HashSet<TType>())
        {
            
        }
        
        public TypeRegistry(IEnumerable<TType>  collection) : this(new HashSet<TType>(collection))
        {
            
        }
        
        public TypeRegistry(HashSet<TType> collection)
        {
            _collection =  collection;
        }

        public bool Add(TType instance)
        {
            return _collection.Add(instance);
        }

        public bool Remove(TType toRemove)
        {
            return _collection.Remove(toRemove);
        }
        
        public T Get<T>()
        {
            foreach (var x in _collection)
            {
                if (x is T t) return t;
            }

            return default;
        }

        public bool TryGet<T>(out T result)
        {
            foreach (var x in _collection)
            {
                if (x is T t)
                {
                    result = t;
                    
                    return true;
                }
            }
            
            result = default;
            
            return false;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _collection.OfType<T>();
        }

        public bool TryGetAll<T>(out IEnumerable<T> results)
        {
            results = GetAll<T>();
            return results != null && results.Any();
        }

        public bool HasOfType<T>()
        {
            return _collection.Any(x => x is T);
        }

        public int GetCount<T>()
        {
            return _collection.Count(x => x is T);
        }
    }
}