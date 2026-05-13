using IM.SaveSystem;
using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(ISceneLoadContextUser))]
    public class InitializeOnKeyPressed : MonoBehaviour
    {
        [SerializeField] private KeyCode _keyCode = KeyCode.Space;
        private ISceneLoadContextUser _sceneLoadContextUser;

        private void Awake()
        {
            _sceneLoadContextUser = GetComponent<ISceneLoadContextUser>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(_keyCode))
            {
                _sceneLoadContextUser.LoadNew();
            }
        }
    }
}