using System;
using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoom : IRoom
    {
        private readonly SmartCollection<IRoomVisitor> _roomVisitors = new();
        private readonly SmartCollection<IRoomWalker> _roomWalkers = new();
        private bool _isActive;

        public IEnumerable<IRoomVisitor> RoomVisitors => _roomVisitors;
        public IEnumerable<IRoomWalker> RoomWalkers => _roomWalkers;

        public bool IsActive
        {
            get => _isActive;
            private set
            {
                if (_isActive == value) return;
                _isActive = value;
                SetActives(_isActive);
            }
        }

        public bool Add(IRoomVisitor roomVisitor)
        {
            if (roomVisitor.CurrentRoom != null) 
                throw new ArgumentException($"Visitor ({roomVisitor}) must be removed from the other room before adding.");

            if (roomVisitor is not MonoBehaviour mb)
            {
                return AddSingle(roomVisitor);
            }

            var nestedVisitors = mb.GetComponentsInChildren<IRoomVisitor>(true);
            bool anyAdded = false;
            
            foreach (var visitor in nestedVisitors)
            {
                if (AddSingle(visitor)) anyAdded = true;
            }

            return anyAdded;
        }

        public bool Remove(IRoomVisitor roomVisitor)
        {
            if (roomVisitor is not MonoBehaviour mb)
            {
                return RemoveSingle(roomVisitor);
            }

            var nestedVisitors = mb.GetComponentsInChildren<IRoomVisitor>(true);
            bool anyRemoved = false;

            foreach (var visitor in nestedVisitors)
            {
                if (RemoveSingle(visitor)) anyRemoved = true;
            }

            return anyRemoved;
        }

        private bool AddSingle(IRoomVisitor visitor)
        {
            if (_roomVisitors.Contains(visitor)) return false;

            _roomVisitors.Add(visitor);
            visitor.ActiveInRoom = _isActive;
            visitor.CurrentRoom = this;
            return true;
        }

        private bool RemoveSingle(IRoomVisitor visitor)
        {
            if (!_roomVisitors.Remove(visitor)) return false;

            visitor.CurrentRoom = null;
            return true;
        }

        public void Enter(IRoomWalker roomWalker)
        {
            _roomWalkers.Add(roomWalker);
            IsActive = true;
        }

        public void Exit(IRoomWalker roomWalker)
        {
            _roomWalkers.Remove(roomWalker);
            if (_roomWalkers.Count == 0) IsActive = false;
        }

        private void SetActives(bool value)
        {
            foreach (IRoomVisitor roomVisitor in _roomVisitors) 
                roomVisitor.ActiveInRoom = value;
        }
    }
}