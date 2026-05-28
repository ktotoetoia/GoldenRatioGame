using IM.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    [DefaultExecutionOrder(-100)]
    public class SaveOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private string _buttonName = "SaveButton";
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_buttonName).clicked += Clicked;
        }

        private void Clicked()
        {
            _gameInfoController.Save();
        }
    }
}