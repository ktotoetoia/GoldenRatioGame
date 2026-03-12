using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.SaveSystem
{
    //test
    public class SceneIdInjector : MonoBehaviour
    {
        private void Awake()
        {
            InjectIds();
        }

        [ContextMenu("Inject IDs")]
        public void InjectIds()
        {
            Debug.Log("=== Injecting Scene IDs ===");

            HashSet<string> usedIds = new();

            var identifiables = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IIdentifiable>()
                .ToArray();

            foreach (IIdentifiable ident in identifiables)
            {
                MonoBehaviour mb = ident as MonoBehaviour;
                if (!mb) continue;

                string id = ident.Id;

                // Generate ID if missing or duplicate
                if (string.IsNullOrEmpty(id) || usedIds.Contains(id))
                {
                    id = GenerateId(mb.gameObject);
                    ident.InjectId(id);
                }

                usedIds.Add(id);
            }

            Debug.Log($"Injected IDs for {usedIds.Count} objects.");
        }

        private string GenerateId(GameObject go)
        {
            return $"{go.name}_{Guid.NewGuid():N}";
        }
    }
}