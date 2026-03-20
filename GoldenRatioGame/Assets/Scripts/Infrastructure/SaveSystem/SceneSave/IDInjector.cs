using System;
using System.Collections.Generic;
using IM.Common;
using UnityEngine;

namespace IM.SaveSystem
{
    public class IDInjector : MonoBehaviour, IGameObjectFactoryObserver
    {
        private readonly HashSet<string> _ids = new();
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            if(deserialized) return;
            
            if (instance.TryGetComponent(out IIdentifiable identifiable))
            {
                if (string.IsNullOrEmpty(identifiable.Id))
                {
                    Inject(identifiable);
                    return;
                }

                if (_ids.Add(identifiable.Id)) return;
                
                Debug.LogWarning($"Duplicate ID: {identifiable.Id}, new id will be injected");
                Inject(identifiable);
            }
        }

        private void Inject(IIdentifiable identifiable)
        {
            string newId = GenerateUnusedID();
                    
            identifiable.InjectId(newId);
                    
            _ids.Add(newId);
        }

        private string GenerateUnusedID()
        {
            string id = Guid.NewGuid().ToString();

            while (_ids.Contains(id)) id = Guid.NewGuid().ToString();

            return id;
        }
    }
}