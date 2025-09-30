using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.Base
{
    public class EnumerableWrapper<TInput, TOutput> : IEnumerable<TOutput>
    {
        private readonly IFactory<TOutput,TInput> _factory;
        private readonly Dictionary<TInput, TOutput> _cache;
        private readonly IEnumerable<TInput> _source;
        
        public IReadOnlyDictionary<TInput, TOutput> Cache => _cache;

        public EnumerableWrapper(IEnumerable<TInput> source, IFactory<TOutput,TInput> factory)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _cache = new Dictionary<TInput, TOutput>();
        }

        public IEnumerator<TOutput> GetEnumerator()
        {
            HashSet<TInput> currentKeys = new HashSet<TInput>();

            foreach (TInput item in _source)
            {
                currentKeys.Add(item);

                if (!_cache.TryGetValue(item, out TOutput value))
                {
                    value = _factory.Create(item);
                    _cache[item] = value;
                }

                yield return value;
            }

            foreach (TInput key in _cache.Keys.Except(currentKeys).ToList())
                _cache.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}