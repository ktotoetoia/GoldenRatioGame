using IM.Events;
using UnityEngine;

namespace IM.Modules
{
    public class ValueStorageContainerMono : MonoBehaviour, IValueStorageContainer, IExtension
    {
        private readonly IValueStorageContainer _valueStorageContainer = new ValueStorageContainer();
        
        public IValueStorage<T> Get<T>(string tag = null)
        {
            return _valueStorageContainer.Get<T>(tag);
        }

        public bool TryGet<T>(out IValueStorage<T> storage, string tag = null)
        {
            return _valueStorageContainer.TryGet(out storage, tag);
        }

        public IValueStorage<T> GetOrCreate<T>(string tag = null)
        {
            return _valueStorageContainer.GetOrCreate<T>(tag);
        }

        public void Remove<T>(string tag = null)
        {
            _valueStorageContainer.Remove<T>(tag);
        }
    }
}