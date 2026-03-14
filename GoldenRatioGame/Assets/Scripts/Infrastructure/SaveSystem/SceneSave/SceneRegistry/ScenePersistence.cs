using System;
using System.Text;
using IM.OdinSerializer;
using UnityEngine;

namespace IM.SaveSystem
{
    internal class ScenePersistence
    {
        private readonly RegistryStore _store;

        public ScenePersistence(RegistryStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public string Serialize()
        {
            var saveFile = CreateSaveFileFromRegistry();
            byte[] bytes = SerializationUtility.SerializeValue(saveFile, DataFormat.JSON);
            return Encoding.UTF8.GetString(bytes);
        }

        private SceneSaveFile CreateSaveFileFromRegistry()
        {
            var save = new SceneSaveFile();
            var snapshot = _store.Snapshot();

            foreach (var kv in snapshot)
            {
                var e = kv.Value;
                if (e.GetSerializer() is { } s)
                {
                    try
                    {
                        save.Objects.Add(s.Capture());
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Capture failed for {e.Id}");
                        Debug.LogException(ex);
                    }
                }
            }

            return save;
        }

        public static SceneSaveFile DeserializeToSaveFile(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            return SerializationUtility.DeserializeValue<SceneSaveFile>(bytes, DataFormat.JSON);
        }
    }
}