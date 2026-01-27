using IM.Modules;
using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [DefaultExecutionOrder(100001)]
    public class DocumentTest : MonoBehaviour
    {
        [SerializeField] private ModuleEntity _moduleEntity;
        [SerializeField] private UIDocument _document;
        private ItemMutableStorageUI _itemMutableStorageUI;
        
        private void Awake()
        {
            _itemMutableStorageUI = _document.rootVisualElement.Q<ItemMutableStorageUI>();
            
            _itemMutableStorageUI.SetStorage(_moduleEntity.ModuleController.Storage as IStorage);
        }
    }
}