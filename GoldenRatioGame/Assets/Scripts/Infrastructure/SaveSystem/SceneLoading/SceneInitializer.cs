using IM.LifeCycle;
using UnityEngine;

namespace IM.SaveSystem
{
    public abstract class SceneInitializer : ScriptableObject
    {
        public abstract void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory);
    }
}