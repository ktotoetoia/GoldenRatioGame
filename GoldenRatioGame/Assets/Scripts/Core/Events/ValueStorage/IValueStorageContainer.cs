namespace IM.Events
{
    public interface IValueStorageContainer
    {
        IValueStorage<T> Get<T>();
        bool TryGet<T>(out IValueStorage<T> storage);

        IValueStorage<T> GetOrCreate<T>();
        void Remove<T>();
    }
}