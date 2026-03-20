using System.Collections.Generic;
using System.IO;
using IM.Common;
using UnityEngine;

namespace IM.SaveSystem
{
    [CreateAssetMenu(menuName = "Scene/Scene Load Context")]
    public class SceneLoadContext : ScriptableObject
    {
        [SerializeField] private string _relativePath;
        [field: SerializeField] public SceneLoadType SceneLoadType { get; set; }
        
        [SerializeField] private List<SceneInitializer> _initializers;
        
        public string FullSceneLoadPath => Path.Combine(Application.persistentDataPath, _relativePath);

        public void OnSceneLoaded(GameObject initializerGO)
        {
            if(SceneLoadType == SceneLoadType.LoadExisting) return;
            
            IGameObjectFactory factory = initializerGO.GetComponent<IGameObjectFactory>();
            
            _initializers.ForEach( i => i.OnSceneLoaded(initializerGO, factory));   
        }
    }
}