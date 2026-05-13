using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Tests
{
    public class LoadSceneOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private string _buttonName = "LoadSceneButton";
        [SerializeField] private int _sceneIndex;
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_buttonName).clicked += Clicked;
        }

        private void Clicked()
        {
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}