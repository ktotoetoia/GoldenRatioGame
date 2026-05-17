using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class LoadLocationOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private string _buttonName = "GiveUpButton";
        [SerializeField] private Location _location;
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_buttonName).clicked += Clicked;
        }

        private void Clicked()
        {
            _gameInfoController.ProgressTo(_location);
        }
    }
}