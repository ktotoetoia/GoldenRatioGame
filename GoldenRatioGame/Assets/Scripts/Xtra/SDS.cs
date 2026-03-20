using IM.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Tests
{
    public class SDS : MonoBehaviour
    {
        [SerializeField] private SceneLoadContext _sceneLoadContext;
        [SerializeField] private int _sceneIndex;
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();

            _document.rootVisualElement.Q<Button>("CreateNewButton").clicked +=CreateNew;
            _document.rootVisualElement.Q<Button>("LoadLastButton").clicked +=LoadLast;
        }

        private void CreateNew()
        {
            _sceneLoadContext.SceneLoadType = SceneLoadType.NewScene;
            SceneManager.LoadScene(_sceneIndex);
        }

        private void LoadLast()
        {
            _sceneLoadContext.SceneLoadType = SceneLoadType.LoadExisting;
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}