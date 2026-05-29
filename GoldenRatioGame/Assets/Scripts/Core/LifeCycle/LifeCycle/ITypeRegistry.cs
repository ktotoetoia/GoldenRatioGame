using System.Collections.Generic;

namespace IM.LifeCycle
{
    public interface ITypeRegistry<out TType> where TType : class
    {
        IReadOnlyCollection<TType> Collection { get; }

        T Get<T>();
        IEnumerable<T> GetAll<T>();
        bool TryGet<T>(out T result);
        bool TryGetAll<T>(out IEnumerable<T> results);

        bool HasOfType<T>();
        int GetCount<T>();
    }

    public interface ITaggedTypeRegistry<out TType>
    {
        
    }
}