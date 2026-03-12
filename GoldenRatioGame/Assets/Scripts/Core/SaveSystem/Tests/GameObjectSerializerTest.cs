using System.IO;
using IM.OdinSerializer;
using UnityEngine;

namespace IM.SaveSystem
{
    public class GameObjectSerializerTest : MonoBehaviour
    {
        private const string SaveFileName = "test_save.txt";
        private static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);
        
        private GameObjectSerializer _serializer;
        
        private void Awake()
        {
            _serializer = GetComponent<GameObjectSerializer>();
        }
        
        [ContextMenu("Save")]
        public void Save()
        {
            GameObjectData data = _serializer.Capture();
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            File.WriteAllBytes(SavePath, bytes);
            Debug.Log($"[SaveTest] Saved to: {SavePath}");
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning($"[SaveTest] No save file found at: {SavePath}");
                return;
            }

            byte[] bytes = File.ReadAllBytes(SavePath);
            GameObjectData data = SerializationUtility.DeserializeValue<GameObjectData>(bytes, DataFormat.Binary);
            _serializer.Restore(data);
            Debug.Log($"[SaveTest] Loaded from: {SavePath}");
        }

        [ContextMenu("Delete Save")]
        public void DeleteSave()
        {
            if (!File.Exists(SavePath)) return;
            File.Delete(SavePath);
            Debug.Log("[SaveTest] Save file deleted.");
        }
    }
}