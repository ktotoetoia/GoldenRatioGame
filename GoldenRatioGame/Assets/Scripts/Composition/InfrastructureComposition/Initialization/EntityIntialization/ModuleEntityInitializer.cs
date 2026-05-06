using System.Collections.Generic;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(menuName = "Initialization/Module Entity Initializer")]
    public class ModuleEntityInitializer : SceneInitializer
    {
        [SerializeField] private List<ModuleEntityEntry> _toInitialize;
        private readonly ModuleEntityFactory _factory = new();
        
        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            foreach (ModuleEntityEntry entry in _toInitialize) _factory.Create(entry,factory);
        }
    }
}