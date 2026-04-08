using System.Collections.Generic;
using System.Linq;
using IM.Factions;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;

namespace IM.Map
{
    public sealed class RoomPortsController : MonoBehaviour
    {
        private readonly IEntityCollection _entities = new EntityCollection();
        private readonly Dictionary<IEntity, IFactionMember> _factionMembers = new();
        private IGameObjectRoom _room;
        private IGameObjectRoomEvents _roomEvents;

        private void Awake()
        {
            _room = GetComponent<IGameObjectRoom>();
            _roomEvents = GetComponent<IGameObjectRoomEvents>();
            _roomEvents.GameObjectAdded += OnVisitorAdded;
            _roomEvents.GameObjectRemoved += OnVisitorRemoved;
            _roomEvents.RoomPortAdded += x => RefreshPortsState();
            _roomEvents.RoomPortRemoved += x => RefreshPortsState();
            
            _entities.EntityAdded += OnEntityChanged;
            _entities.EntityRemoved += OnEntityChanged;
            _entities.EntityDestroyed += OnEntityDestroyed;

            RefreshPortsState();
        }

        private void Update()
        {
            RefreshPortsState();
        }

        private void OnVisitorAdded(GameObject go)
        {
            if (!go.TryGetComponent(out IModuleEntity entity)) return;
            
            if (!entity.GameObject.TryGetComponent(out IFactionMember factionMember)) return;

            if (!_factionMembers.TryAdd(entity, factionMember)) return;

            _entities.Add(entity);
        }

        private void OnVisitorRemoved(GameObject go)
        {
            if (!go.TryGetComponent(out IModuleEntity entity)) return;
            
            _factionMembers.Remove(entity);
            _entities.Remove(entity);
        }

        private void OnEntityChanged(IEntity entity)
        {
            if (!_entities.Contains(entity))
            {
                _factionMembers.Remove(entity);
            }
    
            RefreshPortsState();
        }

        private void OnEntityDestroyed(IEntity entity)
        {
            _factionMembers.Remove(entity);
            RefreshPortsState();
        }

        private void RefreshPortsState()
        {
            bool shouldOpen = true;

            if (_factionMembers.Count > 0)
            {
                IFaction firstFaction = _factionMembers.Values.First().Faction;
                shouldOpen = _factionMembers.Values.All(m => m.Faction == firstFaction);
            }

            foreach (IRoomPort roomPort in _room.RoomPorts)
                roomPort.IsOpen = shouldOpen;
        }

        private void OnDestroy()
        {
            if (_roomEvents != null)
            {
                _roomEvents.GameObjectAdded -= OnVisitorAdded;
                _roomEvents.GameObjectRemoved -= OnVisitorRemoved;
            }

            _entities.EntityAdded -= OnEntityChanged;
            _entities.EntityRemoved -= OnEntityChanged;
            _entities.EntityDestroyed -= OnEntityDestroyed;

            _entities.Clear();
            _factionMembers.Clear();
        }
    }
}