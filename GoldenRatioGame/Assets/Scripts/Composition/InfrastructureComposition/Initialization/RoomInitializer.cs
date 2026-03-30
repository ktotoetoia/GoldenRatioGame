using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using IM.Map;
using UnityEngine;

namespace IM
{
    public class RoomInitializer : MonoBehaviour, IRequireGameObjectFactory
    {
        [SerializeField] private List<ModuleEntityEntry> _toInitialize;
        private readonly ModuleEntityFactory _entityFactory = new();
        private IGameObjectFactory _factory;
        private IRoom _room;
        private bool _initialized;

        public IGameObjectFactory Factory
        {
            get => _factory;
            set
            {
                if (_initialized) return;
                
                _factory = value;
                _initialized = true;
                _room = GetComponent<IRoom>();
                
                foreach (IRoomVisitor roomVisitor in _toInitialize.Select(x => _entityFactory.Create(x, _factory).GameObject.GetComponent<IRoomVisitor>()))
                {
                    _room.Add(roomVisitor);    
                }
            }
        }
    }
}