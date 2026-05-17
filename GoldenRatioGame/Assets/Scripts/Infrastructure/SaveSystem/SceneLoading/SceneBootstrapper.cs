using System.IO;
using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    public class SceneBootstrapper : MonoBehaviour, IHaveSceneRegistry
    {
        [SerializeField] private SceneLoadContext _sceneLoadContext;
        private IGameObjectFactory _factory;
        
        public ISceneRegistry SceneRegistry { get; private set; }
        
        private void Awake()
        {
            _factory = GetComponent<IGameObjectFactory>();
            InitializeScene();
        }
        
        private void InitializeScene()
        {
            SceneRegistry = new SceneRegistry(_factory);
            
            if (_sceneLoadContext.SceneLoadType == SceneLoadType.LoadExisting && !string.IsNullOrEmpty(_sceneLoadContext.FullSceneLoadPath))
            {
                LoadScene(_sceneLoadContext.FullSceneLoadPath);    
                return;
            }
            
            CreateNewScene();
        }
        
        private void LoadScene(string loadPath)
        {
            if (File.Exists(loadPath))
            {
                SceneRegistry.Deserialize(File.ReadAllText(loadPath));
            }
            else
            {
                Debug.LogWarning($"Save file missing at {loadPath}! Creating a default fallback scene layout instead.");
                CreateNewScene();
            }
        }
        
        private void CreateNewScene()
        {
            _sceneLoadContext.OnSceneLoaded(gameObject);
        }

        public string GetSaved()
        {
            return SceneRegistry.Serialize();
        }
        
        [ContextMenu("Save")]
        public void ManualDebugSave()
        {
            if (string.IsNullOrEmpty(_sceneLoadContext.FullSceneLoadPath))
            {
                Debug.LogError("Cannot run debug save: SceneLoadContext path string is empty.");
                return;
            }
            File.WriteAllText(_sceneLoadContext.FullSceneLoadPath, GetSaved());
            Debug.Log($"Debug saved successfully to: {_sceneLoadContext.FullSceneLoadPath}");
        }
        
        [ContextMenu("Print Save Path")]
        private void PrintSavePath()
        {
            Debug.Log(string.IsNullOrEmpty(_sceneLoadContext.FullSceneLoadPath) ? "Path is empty." : _sceneLoadContext.FullSceneLoadPath);
        }
    }
}