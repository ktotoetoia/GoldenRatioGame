using System.Collections.Generic;
using System.IO;
using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    [CreateAssetMenu(menuName = "Scene/Scene Load Context")]
    public class SceneLoadContext : ScriptableObject
    {
        [SerializeField] private string _relativePath;
        [SerializeField] private List<SceneInitializer> _initializers;
        
        [field: SerializeField] public SceneLoadType SceneLoadType { get; set; }
        [field:SerializeField] public int SceneIndex { get; set; }
        
        public string FullSceneLoadPath => Path.Combine(Application.persistentDataPath, _relativePath);

        public void OnSceneLoaded(GameObject initializerGO)
        {
            if(SceneLoadType == SceneLoadType.LoadExisting) return;
            
            IGameObjectFactory factory = initializerGO.GetComponent<IGameObjectFactory>();
            
            _initializers.ForEach( i => i.OnSceneLoaded(initializerGO, factory));   
        }
    }
}