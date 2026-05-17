using System.Collections.Generic;
using IM.LifeCycle;
using IM.Map;
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
            Floor floor = FindAnyObjectByType<Floor>();
            
            foreach (ModuleEntityEntry entry in _toInitialize)
            {
                var entity = _factory.Create(entry,factory);
                if (floor && entity.GameObject.TryGetComponent(out IRoomWalker roomWalker))
                {
                    floor.AddRoomWalker(roomWalker);
                }
            }
        }
    }
}