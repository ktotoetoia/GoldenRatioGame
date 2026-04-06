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

        private IRoom _room;
        private IRoomEvents _roomEvents;

        private void Awake()
        {
            _room = GetComponent<IRoom>();
            _roomEvents = GetComponent<IRoomEvents>();

            _roomEvents.RoomVisitorAdded += OnVisitorAdded;
            _roomEvents.RoomVisitorRemoved += OnVisitorRemoved;
            _roomEvents.RoomPortAdded += x => RefreshPortsState();
            _roomEvents.RoomPortRemoved += x => RefreshPortsState();
            
            _entities.EntityAdded += OnEntityChanged;
            _entities.EntityRemoved += OnEntityChanged;
            _entities.EntityDestroyed += OnEntityDestroyed;

            RefreshPortsState();
        }

        private void OnVisitorAdded(IRoomVisitor visitor)
        {
            if (visitor?.Entity is not IModuleEntity) return;

            if (!visitor.Entity.GameObject.TryGetComponent(out IFactionMember factionMember)) return;

            if (!_factionMembers.TryAdd(visitor.Entity, factionMember)) return;

            _entities.Add(visitor.Entity);
        }

        private void OnVisitorRemoved(IRoomVisitor visitor)
        {
            if (visitor?.Entity == null) return;

            _factionMembers.Remove(visitor.Entity);
            _entities.Remove(visitor.Entity);
        }

        private void OnEntityChanged(IEntity entity)
        {
            SyncFactionMembers();
            RefreshPortsState();
        }

        private void OnEntityDestroyed(IEntity entity)
        {
            _factionMembers.Remove(entity);
            RefreshPortsState();
        }

        private void SyncFactionMembers()
        {
            List<IEntity> toRemove = new List<IEntity>();

            foreach (IEntity entity in _factionMembers.Keys)
            {
                if (!_entities.Contains(entity)) toRemove.Add(entity);
            }

            foreach (IEntity entity in toRemove) _factionMembers.Remove(entity);
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
                _roomEvents.RoomVisitorAdded -= OnVisitorAdded;
                _roomEvents.RoomVisitorRemoved -= OnVisitorRemoved;
            }

            _entities.EntityAdded -= OnEntityChanged;
            _entities.EntityRemoved -= OnEntityChanged;
            _entities.EntityDestroyed -= OnEntityDestroyed;

            _entities.Clear();
            _factionMembers.Clear();
        }
    }
}