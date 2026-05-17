using IM.LifeCycle;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class ChangePauseOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private string _pauseChangeButton = "ContinueButton";
        [SerializeField] private PauseManager _pauseManager;
        private UIDocument _document;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>().clicked += () => _pauseManager.SetPaused(!_pauseManager.Paused)  ;
        }
    }
}