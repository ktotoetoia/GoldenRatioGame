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
            
            if (_sceneLoadContext.SceneLoadType == SceneLoadType.LoadExisting)
            {
                LoadScene(_sceneLoadContext.FullSceneLoadPath);    
                
                return;
            }
            
            CreateNewScene();
        }
        
        private void LoadScene(string loadPath)
        {
            SceneRegistry.Deserialize(File.ReadAllText(loadPath));
        }
        
        private void CreateNewScene()
        {
            _sceneLoadContext.OnSceneLoaded(gameObject);
        }
        
        [ContextMenu("Save")]
        public void Save()
        {
            string json = SceneRegistry.Serialize();
            
            File.WriteAllText(_sceneLoadContext.FullSceneLoadPath, json);
        }
        
        [ContextMenu("Print Save Path")]
        private void PrintSavePath()
        {
            Debug.Log(_sceneLoadContext.FullSceneLoadPath);
        }
    }
}