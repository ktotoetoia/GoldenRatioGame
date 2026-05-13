using System;
using IM.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    [RequireComponent(typeof(ISceneLoadContextUser))]
    public class InitializeSceneOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private string _initializeButton = "ExitButton";
        [SerializeField] private bool _load;
        private ISceneLoadContextUser _sceneLoadContextUser;
        private UIDocument _document;
        
        private void Awake()
        {
            _sceneLoadContextUser = GetComponent<ISceneLoadContextUser>();
            _document = GetComponent<UIDocument>();
            Button button = _document.rootVisualElement.Q<Button>(_initializeButton);

            button.clicked += _load ? _sceneLoadContextUser.LoadLast : _sceneLoadContextUser.LoadNew;
        }
    }
}