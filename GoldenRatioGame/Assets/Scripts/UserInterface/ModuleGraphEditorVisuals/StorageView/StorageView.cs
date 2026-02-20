using System.Linq;
using IM.Storages;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals.Graph
{
    public class StorageView : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private IStorageVisual _storageVisual;

        public IStorageVisual StorageVisual
        {
            get
            {
                return _storageVisual ??= _document.rootVisualElement.Query().ToList().FirstOrDefault(x => x is IStorageVisual) as IStorageVisual;
            }
        }

        private void Awake()
        {
            _document.rootVisualElement.visible = false;
        }

        public void SetStorage(IReadOnlyStorage storage)
        {
            StorageVisual.SetStorage(storage);
            _document.rootVisualElement.visible = true;
        }

        public void ClearStorage()
        {
            StorageVisual.ClearStorage();
            _document.rootVisualElement.visible = false;
        }
    }
}