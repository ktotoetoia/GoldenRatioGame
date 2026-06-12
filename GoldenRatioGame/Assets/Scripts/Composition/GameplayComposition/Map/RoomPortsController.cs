using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using IM.Factions;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;

namespace IM.Map
{
    public sealed class RoomPortsController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _transitionPointsSources;
        [SerializeField] private int _augmentProgressValueOnClear = 1;
        [SerializeField] private Faction _faction;
        private readonly IEntityCollection _entities = new EntityCollection();
        private readonly Dictionary<IEntity, IFactionMember> _factionMembers = new();
        private readonly List<ITransitionPoint> _transitionPoints = new ();
        private IGameObjectRoom _room;
        private IGameObjectRoomEvents _roomEvents;
        
        public bool Cleared { get; set; }

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

            foreach (GameObject transitionPointsSource in _transitionPointsSources)
            {
                if (transitionPointsSource.TryGetComponent(out ITransitionPoint transitionPoint))
                {
                    _transitionPoints.Add(transitionPoint);
                }
            }
            
            foreach (var roomGameObject in _room.GameObjects)
            {
                OnVisitorAdded(roomGameObject);
            }
            
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
                shouldOpen = _factionMembers.Values.All(m => ReferenceEquals(m.Faction, _faction));
            }
            
            if (_factionMembers.Any() && shouldOpen && !Cleared)
            {
                Cleared = true;
                
                foreach (KeyValuePair<IEntity, IFactionMember> keyValuePair in _factionMembers)
                {
                    if (keyValuePair.Key.GameObject.TryGetComponent(out IAugmentProgressManager progressManager))
                    {
                        progressManager.Progress(_augmentProgressValueOnClear);
                    }
                }
            }
            
            foreach (IRoomPort roomPort in _room.RoomPorts)
                roomPort.IsOpen = shouldOpen;
            foreach (ITransitionPoint transitionPoint in _transitionPoints) 
                transitionPoint.IsOpen = shouldOpen;
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