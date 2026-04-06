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

        public bool Add(IRoomVisitor visitor)
        {
            if (visitor.CurrentRoom != null || _roomVisitors.Contains(visitor)) return false;

            _roomVisitors.Add(visitor);
            visitor.CurrentRoom = this;
            return true;
        }

        public bool Remove(IRoomVisitor visitor)
        {
            if (!_roomVisitors.Remove(visitor)) return false;

            visitor.CurrentRoom = null;
            return true;
        }

        private void SetActives(bool value)
        {
            foreach (IRoomVisitor roomVisitor in _roomVisitors)
            {
                roomVisitor.ActiveInRoom = roomVisitor is IRoomActivator || value;
            }
        }

        public bool Add(IRoomPort roomPort)
        {
            if (_roomPorts.Contains(roomPort)) return false;
            _roomPorts.Add(roomPort);
            return true;
        }

        public bool Remove(IRoomPort roomPort)
        {
            return _roomPorts.Remove(roomPort);
        }
    }
}