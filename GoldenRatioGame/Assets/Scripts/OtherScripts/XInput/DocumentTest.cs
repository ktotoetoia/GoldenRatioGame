using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class DocumentTest : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private ItemMutableStorageUI _itemMutableStorageUI;
        
        public void SetStorage(IReadOnlyStorage storage)
        {
            _itemMutableStorageUI ??= _document.rootVisualElement.Q<ItemMutableStorageUI>();
            
            _itemMutableStorageUI.SetStorage(storage);
        }
    }
}