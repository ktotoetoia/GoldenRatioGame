using IM.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class DocumentTest : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private ItemMutableStorageUI _itemMutableStorageUI;
        
        private void Awake()
        {
            _itemMutableStorageUI = _document.rootVisualElement.Q<ItemMutableStorageUI>();
            
            _itemMutableStorageUI.SetStorage(GetComponent<ModuleEntityTestInput>().Storage);
        }
    }
}

