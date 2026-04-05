using System;
using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoomMono : MonoBehaviour, IRoom, IRoomEvents
    {
        private readonly GameObjectRoom _room = new();

        public bool IsActive => _room.IsActive;

        public event Action<IRoomVisitor> RoomVisitorAdded;
        public event Action<IRoomVisitor> RoomVisitorRemoved;
        public event Action<IRoomPort> RoomPortAdded;
        public event Action<IRoomPort> RoomPortRemoved;
        public event Action<IRoomWalker> RoomWalkerEnter;
        public event Action<IRoomWalker> RoomWalkerExit;

        public IEnumerable<IRoomVisitor> RoomVisitors => _room.RoomVisitors;
        public IEnumerable<IRoomPort> RoomPorts => _room.RoomPorts;

        private void Awake()
        {
            SyncActiveState();
        }

        public bool Add(IRoomVisitor roomVisitor)
        {
            if (!_room.Add(roomVisitor))
                return false;

            SyncActiveState();
            AttachToRoom(roomVisitor);
            RoomVisitorAdded?.Invoke(roomVisitor);
            return true;
        }

        public bool Remove(IRoomVisitor roomVisitor)
        {
            if (!_room.Remove(roomVisitor))
                return false;

            SyncActiveState();
            DetachFromRoom(roomVisitor);
            RoomVisitorRemoved?.Invoke(roomVisitor);
            return true;
        }

        public bool Add(IRoomPort roomPort)
        {
            if (!_room.Add(roomPort))
                return false;

            AttachToRoom(roomPort);
            RoomPortAdded?.Invoke(roomPort);
            return true;
        }

        public bool Remove(IRoomPort roomPort)
        {
            if (!_room.Remove(roomPort))
                return false;

            DetachFromRoom(roomPort);
            RoomPortRemoved?.Invoke(roomPort);
            return true;
        }

        private void SyncActiveState()
        {
            gameObject.SetActive(_room.IsActive);
        }

        private void AttachToRoom(object roomObject)
        {
            if (roomObject is not MonoBehaviour monoBehaviour)
                return;

            monoBehaviour.transform.SetParent(transform, worldPositionStays: true);
        }

        private static void DetachFromRoom(object roomObject)
        {
            if (roomObject is not MonoBehaviour monoBehaviour)
                return;

            if (monoBehaviour.TryGetComponent(out IParentRestorable restorable))
                restorable.ResetToDefaultParent();
            else
                monoBehaviour.transform.SetParent(null, worldPositionStays: true);
        }
    }
}