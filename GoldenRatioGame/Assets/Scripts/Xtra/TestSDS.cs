using IM.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Tests
{
    public class TestSDS : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        [SerializeField] private SceneBootstrapper _sceneBootstrapper;
        private UIDocument _document;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.focusable = false;

            _document.rootVisualElement.Q<Button>("Save").clicked += () =>
            {
                _sceneBootstrapper.Save();
            };

            _document.rootVisualElement.Q<Button>("Exit").clicked += () =>
            {
                SceneManager.LoadScene(_sceneIndex);
            };
        }
    }
}