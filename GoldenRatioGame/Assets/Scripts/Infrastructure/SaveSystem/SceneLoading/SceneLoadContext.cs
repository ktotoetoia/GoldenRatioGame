using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    [CreateAssetMenu(menuName = "Scene/Scene Load Context")]
    public class SceneLoadContext : ScriptableObject
    {
        [SerializeField] private List<SceneInitializer> _initializers;
        
        [field: SerializeField] public SceneLoadType SceneLoadType { get; set; }
        [field: SerializeField] public int SceneIndex { get; set; }
        [field: SerializeField] public string FullSceneLoadPath { get; set; } 

        public void OnSceneLoaded(GameObject initializerGO)
        {
            if (SceneLoadType == SceneLoadType.LoadExisting) return;
            
            var factory = initializerGO.GetComponent<IGameObjectFactory>();
            if (factory == null) return;
            
            foreach (var initializer in _initializers)
            {
                initializer.OnSceneLoaded(initializerGO, factory);
            }
        }

        public void ResetContext()
        {
            FullSceneLoadPath = string.Empty;
            SceneLoadType = SceneLoadType.None;
            SceneIndex = 0;
        }
    }
}