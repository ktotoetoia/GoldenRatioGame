using IM.Health;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class GameObjectHealthDisplay : MonoBehaviour, IGameObjectStatusDisplay
    {
        [SerializeField] private Vector3 _offset = new (0,1);
        private UIDocument _document;
        private CappedValueElement  _element;
        private GameObject _displayed;
        
        public GameObject Displayed
        {
            get => _displayed;
            set
            {
                if(_displayed == value) return;
                
                _displayed = value;
                
                IFloatHealth health = _displayed?.GetComponent<IFloatHealth>();
                
                if(health == null) return;
                
                _element.GetCappedValue = () => health.Health;
            }
        }

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _element = _document.rootVisualElement.Q<CappedValueElement>();
        }

        private void Update()
        {
            if (!_displayed)
            {
                _document.rootVisualElement.visible = false;
                return;
            }

            _element?.Update();
            _document.rootVisualElement.visible = true;
            transform.position = _displayed.transform.position + _offset;
        }
    }
}