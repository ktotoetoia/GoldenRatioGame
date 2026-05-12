using IM.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class SaveOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private SceneBootstrapper _sceneBootstrapper;
        [SerializeField] private string _buttonName = "SaveButton";
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_buttonName).clicked += Clicked;
        }

        private void Clicked()
        {
            _sceneBootstrapper.Save();
        }
    }
}