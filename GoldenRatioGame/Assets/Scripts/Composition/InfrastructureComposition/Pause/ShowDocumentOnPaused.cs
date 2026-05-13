using IM;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class ShowDocumentOnPaused : MonoBehaviour, IPausable
    {
        [SerializeField] private bool _showOnPause = true;
        private bool _paused;
        private UIDocument _document;
        
        public bool Paused
        {
            get => _paused;
            set
            {
                _paused = value;
                _document ??= GetComponent<UIDocument>();
                
                _document.rootVisualElement.visible =  _paused == _showOnPause;
            }
        }

        private void Awake()
        {
            Paused = Paused;
        }
    }
}