using System;
using System.Linq;
using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class StorageView : MonoBehaviour
    {
        private UIDocument _document;
        private IStorageElement _storageElement;

        public IStorageElement StorageElement
        {
            get
            {
                return _storageElement ??= _document.rootVisualElement.Query().ToList().FirstOrDefault(x => x is IStorageElement) as IStorageElement;
            }
        }

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
        }

        public void SetStorage(IReadOnlyStorage storage)
        {
            if (storage is not IStorageEvents events)
            {
                throw new ArgumentException("storage must implement IStorageEvents to be used in storage view");
            }
            
            StorageElement.SetStorage(storage,events);
            _document.rootVisualElement.visible = true;
        }

        public void ClearStorage()
        {
            StorageElement.ClearStorage();
            _document.rootVisualElement.visible = false;
        }
    }
}