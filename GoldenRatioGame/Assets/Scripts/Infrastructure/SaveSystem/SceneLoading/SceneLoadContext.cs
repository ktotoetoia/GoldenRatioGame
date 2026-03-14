using System.IO;
using UnityEngine;

namespace IM.SaveSystem
{
    [CreateAssetMenu(menuName = "Scene/Scene Load Context")]
    public class SceneLoadContext : ScriptableObject
    {
        [SerializeField] private string _relativePath;
        
        [field: SerializeField] public SceneLoadType SceneLoadType { get; set; }
        public string FullSceneLoadPath => Path.Combine(Application.persistentDataPath, _relativePath);
    }
}