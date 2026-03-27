using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using IM.Map;
using UnityEngine;

namespace IM
{
    public class RoomInitializer : MonoBehaviour, IRoomInitializer
    {
        [SerializeField] private List<ModuleEntityEntry> _toInitialize;
        [SerializeField] private GameObjectFactory _factory;
        private readonly ModuleEntityFactory _entityFactory = new();
        
        public IEnumerable<GameObject> Initialize(IRoom room)
        {
            return _toInitialize.Select(x => _entityFactory.Create(x, _factory).GameObject);
        }
    }
}