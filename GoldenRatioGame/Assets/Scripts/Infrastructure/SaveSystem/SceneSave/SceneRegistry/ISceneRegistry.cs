using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.SaveSystem
{
    public interface ISceneRegistry
    {
        event Action<string> OnRegistered;
        event Action<string> OnUnregistered;
        string Serialize();
        void ApplySavedObjects(IReadOnlyList<GameObjectData> savedObjects, IPrefabResolver resolver = null, bool instantiateMissing = true);
        bool Register(GameObject go);
        bool Unregister(GameObject go);
        bool Unregister(string id);
        GameObject GetById(string id);
        bool Contains(string id);
        bool Contains(GameObject go);
        void Deserialize(string json);
    }
}