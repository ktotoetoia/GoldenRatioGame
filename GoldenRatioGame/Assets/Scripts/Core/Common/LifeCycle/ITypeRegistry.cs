using System.Collections.Generic;

namespace IM.Modules
{
    public interface ITypeRegistry<out TType> where TType : class
    {
        IReadOnlyCollection<TType> Collection { get; }

        T Get<T>();
        bool TryGet<T>(out T result);

        IEnumerable<T> GetAll<T>();
        bool TryGetAll<T>(out IEnumerable<T> results);

        bool HasOfType<T>();
        int GetCount<T>();
    }
}