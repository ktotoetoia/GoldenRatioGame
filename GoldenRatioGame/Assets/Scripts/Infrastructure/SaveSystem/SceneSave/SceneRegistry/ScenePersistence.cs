using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IM.OdinSerializer;
using UnityEngine;

namespace IM.SaveSystem
{
    internal class ScenePersistence
    {
        private readonly RegistryStore _store;
        private const bool FormatAsReadable = true;
        
        public ScenePersistence(RegistryStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public string Serialize()
        {
            SceneSaveFile saveFile = CreateSaveFileFromRegistry();
    
            using (MemoryStream stream = new MemoryStream())
            {
                using (JsonDataWriter writer = new JsonDataWriter(stream, null, formatAsReadable: FormatAsReadable))
                {
                    SerializationUtility.SerializeValue(saveFile, writer);
                }
        
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private SceneSaveFile CreateSaveFileFromRegistry()
        {
            SceneSaveFile save = new SceneSaveFile();
            KeyValuePair<string, RegistryEntry>[] snapshot = _store.Snapshot();
            
            foreach (KeyValuePair<string, RegistryEntry> kv in snapshot)
            {
                RegistryEntry e = kv.Value;
                
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
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return SerializationUtility.DeserializeValue<SceneSaveFile>(bytes, DataFormat.JSON);
        }
    }
}