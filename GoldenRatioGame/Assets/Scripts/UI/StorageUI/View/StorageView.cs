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
        
        private UIDocument Document => _document ??= GetComponent<UIDocument>();
        public IStorageElement StorageElement => _storageElement ??=
            Document.rootVisualElement.Query().ToList().FirstOrDefault(x => x is IStorageElement) as IStorageElement;

        private void Awake()
        {
            Document.rootVisualElement.visible = false;
        }

        public void SetStorage(IReadOnlyStorage storage)
        {
            if (storage is not IStorageEvents events)
                throw new ArgumentException("storage must implement IStorageEvents to be used in storage view");
            
            StorageElement.SetStorage(storage,events);
            Document.rootVisualElement.visible = true;
        }

        public void ClearStorage()
        {
            StorageElement.ClearStorage();
            Document.rootVisualElement.visible = false;
        }
    }
}