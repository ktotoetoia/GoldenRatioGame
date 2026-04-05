using System;
using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
  public class GameObjectRoom : IRoom
    {
        private readonly SmartCollection<IRoomVisitor> _roomVisitors = new();
        private readonly SmartCollection<IRoomPort> _roomPorts = new();
        private bool _isActive;

        public IEnumerable<IRoomVisitor> RoomVisitors => _roomVisitors;
        public IEnumerable<IRoomPort> RoomPorts => _roomPorts;

        public IEnumerable<GameObject> GameObjects
        {
            get
            {
                foreach (Component roomVisitor in _roomVisitors.OfType<Component>())
                {
                    yield return roomVisitor.gameObject;
                }
            }
        }

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

            bool result;
            if (roomVisitor is not MonoBehaviour mb)
            {
                result = AddSingle(roomVisitor);
            }
            else
            {
                var nestedVisitors = mb.GetComponentsInChildren<IRoomVisitor>(true);
                bool anyAdded = false;
                foreach (var visitor in nestedVisitors)
                {
                    if (AddSingle(visitor)) anyAdded = true;
                }
                result = anyAdded;
            }

            if (result) UpdateActivationState();
            return result;
        }

        public bool Remove(IRoomVisitor roomVisitor)
        {
            bool result;
            if (roomVisitor is not MonoBehaviour mb)
            {
                result = RemoveSingle(roomVisitor);
            }
            else
            {
                var nestedVisitors = mb.GetComponentsInChildren<IRoomVisitor>(true);
                bool anyRemoved = false;
                foreach (var visitor in nestedVisitors)
                {
                    if (RemoveSingle(visitor)) anyRemoved = true;
                }
                result = anyRemoved;
            }

            if (result) UpdateActivationState();
            return result;
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
            visitor.ActiveInRoom = false; 
            return true;
        }

        public void UpdateActivationState()
        {
            IsActive = _roomVisitors.OfType<IRoomActivator>().Any(activator => activator.ShouldActivate);
        }

        private void SetActives(bool value)
        {
            foreach (IRoomVisitor roomVisitor in _roomVisitors) 
                roomVisitor.ActiveInRoom = value;
        }

        public bool Add(IRoomPort roomPort)
        {
            if (_roomPorts.Contains(roomPort)) return false;
            _roomPorts.Add(roomPort);
            return true;
        }

        public bool Remove(IRoomPort roomPort)
        {
            return _roomPorts.Contains(roomPort) && _roomPorts.Remove(roomPort);
        }
    }
}