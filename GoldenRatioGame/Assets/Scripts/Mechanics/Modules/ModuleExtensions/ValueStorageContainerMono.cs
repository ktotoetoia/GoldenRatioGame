using IM.Modules;
using UnityEngine;

namespace TDS.Events
{
    public class ValueStorageContainerMono : MonoBehaviour, IValueStorageContainer, IExtension
    {
        private readonly IValueStorageContainer _valueStorageContainer = new ValueStorageContainer();
        
        public IValueStorage<T> Get<T>()
        {
            return _valueStorageContainer.Get<T>();
        }

        public bool TryGet<T>(out IValueStorage<T> storage)
        {
            return _valueStorageContainer.TryGet(out storage);
        }

        public IValueStorage<T> GetOrCreate<T>()
        {
            return _valueStorageContainer.GetOrCreate<T>();
        }

        public void Remove<T>()
        {
            _valueStorageContainer.Remove<T>();
        }
    }
}