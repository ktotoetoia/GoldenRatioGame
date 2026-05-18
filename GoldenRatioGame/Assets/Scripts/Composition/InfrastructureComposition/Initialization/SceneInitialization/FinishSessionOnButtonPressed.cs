using IM.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class FinishSessionOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private string _buttonName = "QuitButton";
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_buttonName).clicked += Clicked;
        }
        
        private void Clicked()
        {
            _gameInfoController.FinishSession();
        }
    }
}