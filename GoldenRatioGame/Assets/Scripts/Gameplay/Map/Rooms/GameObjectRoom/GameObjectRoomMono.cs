using System;
using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [SelectionBase]
    public class GameObjectRoomMono : MonoBehaviour, IGameObjectRoom, IGameObjectRoomEvents
    {
        private readonly SmartCollection<IRoomPort> _roomPorts = new();
        private readonly SmartCollection<GameObject> _gameObjects = new();
        
        public IEnumerable<GameObject> GameObjects => _gameObjects;
        public IEnumerable<IRoomPort> RoomPorts => _roomPorts;
        public bool IsActive { get; private set; }

        public event Action<GameObject> GameObjectAdded;
        public event Action<GameObject> GameObjectRemoved;
        public event Action<IRoomPort> RoomPortAdded;
        public event Action<IRoomPort> RoomPortRemoved;

        private void Awake()
        {
            IsActive = gameObject.activeInHierarchy;
            UpdateRoomActivation();
        }
        
        public bool Add(IRoomPort roomPort)
        {
            if (_roomPorts.Contains(roomPort)) return false;

            _roomPorts.Add(roomPort);
            AttachToRoom(roomPort);
            RoomPortAdded?.Invoke(roomPort);
            return true;
        }

        public bool Remove(IRoomPort roomPort)
        {
            if (!_roomPorts.Remove(roomPort)) return false;

            DetachFromRoom(roomPort);
            RoomPortRemoved?.Invoke(roomPort);
            return true;
        }

        public bool Add(GameObject toAdd)
        {
            if (!toAdd || _gameObjects.Contains(toAdd)) return false;

            _gameObjects.Add(toAdd);
            if (toAdd.TryGetComponent(out IRoomVisitor visitor))
                visitor.CurrentRoom = this;

            if (toAdd.transform.parent != transform)
                toAdd.transform.SetParent(transform);

            GameObjectAdded?.Invoke(toAdd);
            UpdateRoomActivation();

            return true;
        }

        public bool Remove(GameObject toRemove)
        {
            if (!_gameObjects.Remove(toRemove)) return false;

            if (toRemove.TryGetComponent(out IRoomVisitor visitor) && visitor.CurrentRoom.Equals(this))
                visitor.CurrentRoom = null;

            GameObjectRemoved?.Invoke(toRemove);
            UpdateRoomActivation();

            return true;
        }

        private void UpdateRoomActivation()
        {
            bool shouldBeActive = _gameObjects.Any(x =>
                x.TryGetComponent(out IRoomActivator activator) && activator.ShouldActivate);

            if (IsActive == shouldBeActive) return;
            
            IsActive = shouldBeActive;
            gameObject.SetActive(IsActive);
        }

        private void AttachToRoom(object roomObject)
        {
            if (roomObject is MonoBehaviour mono && mono.transform.parent != transform)
                mono.transform.SetParent(transform, true);
        }

        private void DetachFromRoom(object roomObject)
        {
            if (roomObject is not MonoBehaviour mono) return;

            if (mono.TryGetComponent(out IParentRestorable restorable)) restorable.ResetToDefaultParent();
            else if (mono.transform.parent == transform) mono.transform.SetParent(null, true);
        }

        private void OnTransformChildrenChanged()
        {
            HashSet<GameObject> currentChildren = new HashSet<GameObject>();
            for (int i = 0; i < transform.childCount; i++) currentChildren.Add(transform.GetChild(i).gameObject);

            List<GameObject> toRemove = _gameObjects.Where(go => !currentChildren.Contains(go)).ToList();
            
            foreach (GameObject go in toRemove) Remove(go);

            foreach (GameObject child in currentChildren)
            {
                if (!_gameObjects.Contains(child)) Add(child);
            }
        }
    }
}