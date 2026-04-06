using System;
using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoomMono : MonoBehaviour, IRoom, IRoomEvents
    {
        private readonly SmartCollection<IRoomVisitor> _roomVisitors = new();
        private readonly SmartCollection<IRoomPort> _roomPorts = new();

        public bool IsActive { get; private set; }

        public IEnumerable<IRoomVisitor> RoomVisitors => _roomVisitors;
        public IEnumerable<IRoomPort> RoomPorts => _roomPorts;

        public event Action<IRoomVisitor> RoomVisitorAdded;
        public event Action<IRoomVisitor> RoomVisitorRemoved;
        public event Action<IRoomPort> RoomPortAdded;
        public event Action<IRoomPort> RoomPortRemoved;

        private void Awake()
        {
            UpdateRoomActivation();
        }
        
        public bool Add(IRoomVisitor roomVisitor)
        {
            if (roomVisitor.CurrentRoom != null || _roomVisitors.Contains(roomVisitor)) return false;
            
            _roomVisitors.Add(roomVisitor);
            roomVisitor.CurrentRoom = this;

            UpdateRoomActivation();
            AttachToRoom(roomVisitor);
            
            RoomVisitorAdded?.Invoke(roomVisitor);
            return true;
        }

        public bool Remove(IRoomVisitor roomVisitor)
        {
            if (!_roomVisitors.Remove(roomVisitor)) return false;

            roomVisitor.CurrentRoom = null;

            UpdateRoomActivation();
            DetachFromRoom(roomVisitor);
            
            RoomVisitorRemoved?.Invoke(roomVisitor);
            return true;
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

        private void UpdateRoomActivation()
        {
            bool shouldBeActive = _roomVisitors.Any(v => v is IRoomActivator { ShouldActivate: true });
            
            IsActive = shouldBeActive;

            gameObject.SetActive(IsActive);

            foreach (IRoomVisitor visitor in _roomVisitors)
            {
                visitor.ActiveInRoom = IsActive;
            }
        }

        private void AttachToRoom(object roomObject)
        {
            if (roomObject is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.transform.SetParent(transform, true);
            }
        }

        private static void DetachFromRoom(object roomObject)
        {
            if (roomObject is not MonoBehaviour monoBehaviour) return;

            if (monoBehaviour.TryGetComponent(out IParentRestorable restorable))
            {
                restorable.ResetToDefaultParent();
                return;
            }
            
            monoBehaviour.transform.SetParent(null, true);
        }
    }
}