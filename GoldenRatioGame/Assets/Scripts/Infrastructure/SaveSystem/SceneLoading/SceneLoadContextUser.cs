using UnityEngine;
using UnityEngine.SceneManagement;

namespace IM.SaveSystem
{
    public class SceneLoadContextUser : MonoBehaviour, ISceneLoadContextUser
    {
        [SerializeField] private SceneLoadContext _sceneLoadContext;
        
        public void LoadNew()
        {
            _sceneLoadContext.SceneLoadType = SceneLoadType.NewScene;
            SceneManager.LoadScene(_sceneLoadContext.SceneIndex);
        }

        public void LoadLast()
        {
            _sceneLoadContext.SceneLoadType = SceneLoadType.LoadExisting;
            SceneManager.LoadScene(_sceneLoadContext.SceneIndex);
        }
    }
}