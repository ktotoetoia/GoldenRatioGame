using System.IO;
using System.Linq;
using IM.Common;
using IM.OdinSerializer;
using UnityEngine;

namespace IM.SaveSystem
{
    public class SerializationGameObjectObserver : MonoBehaviour, IGameObjectFactoryObserver
    {
        private IGameObjectFactory _factory;
        private ISceneRegistry _registry;

        private void Awake()
        {
            _factory = GetComponent<IGameObjectFactory>();
            _registry = new SceneRegistry(_factory);
        }

        public void OnCreate(GameObject instance)
        {
            if (instance.TryGetComponent(out IIdentifiable identifiable))
            {
                _registry.Register(instance);
            }
        }
        
        [SerializeField] private string fileName = "scene_save.json";
        private string SavePath => Path.Combine(Application.persistentDataPath, fileName);

        [ContextMenu("Save Scene")]
        public void SaveScene()
        {
            Debug.Log("=== SAVE START ===");

            _registry = new SceneRegistry();

            IIdentifiable[] identifiables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None)
                .OfType<IIdentifiable>()
                .ToArray();

            foreach (IIdentifiable ident in identifiables)
            {
                GameObject go = (ident as MonoBehaviour).gameObject;
                
                if (!_registry.Register(go))
                    Debug.LogWarning($"Failed to register {go.name}");
            }

            string json = _registry.Serialize();

            File.WriteAllText(SavePath, json);

            Debug.Log($"Scene saved to: {SavePath}");
            Debug.Log(json);
        }

        [ContextMenu("Load Scene")]
        public void LoadScene()
        {
            Debug.Log("=== LOAD START ===");

            if (!File.Exists(SavePath))
            {
                Debug.LogError("Save file not found.");
                return;
            }

            string json = File.ReadAllText(SavePath);

            SceneRegistry loadedRegistry = SceneRegistry.Deserialize(json,_factory);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            SceneSaveFile saveFile = SerializationUtility.DeserializeValue<SceneSaveFile>(bytes, DataFormat.JSON);

            loadedRegistry.ApplySavedObjects(saveFile.Objects);

            Debug.Log("Scene loaded.");
        }

        [ContextMenu("Print Save Path")]
        public void PrintPath()
        {
            Debug.Log(SavePath);
        }
    }
}