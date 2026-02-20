namespace IM.Events
{
    public interface IValueStorageContainer
    {
        IValueStorage<T> Get<T>(string tag = null);
        bool TryGet<T>(out IValueStorage<T> storage, string tag = null);

        IValueStorage<T> GetOrCreate<T>(string tag = null);
        void Remove<T>(string tag = null);
    }
}