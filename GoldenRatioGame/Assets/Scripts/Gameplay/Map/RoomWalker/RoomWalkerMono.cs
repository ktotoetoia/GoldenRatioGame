using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class RoomWalkerMono : MonoBehaviour, IRoomWalker
    {
        [SerializeField] private float _roomMovingCooldown = 1f;
        private float _lastDoorMoved = float.MinValue;
        private IRoomVisitor _roomVisitor;

        public IRoom Current { get; private set; }

        public IEnumerable<IRoomPort> AvailablePorts
        {
            get
            {
                if (Current == null || !CooldownFinished()) 
                    return Enumerable.Empty<IRoomPort>();

                return Current.RoomPorts.Where(port => port.IsOpen);
            }
        }

        public IEnumerable<IRoom> Available => AvailablePorts.Select(p => p.Connection.Origin);

        private void Awake() => _roomVisitor = GetComponent<IRoomVisitor>();

        public void GoTo(IRoom room)
        {
            if (Current != null && !CanMoveTo(room)) return;

            _lastDoorMoved = Time.time;
            
            Current?.Remove(_roomVisitor);
            Current = room;
            Current.Add(_roomVisitor);
        }

        private bool CanMoveTo(IRoom room)
        {
            return Available.Any(r => r == room);
        }

        private bool CooldownFinished()
        {
            return Time.time >= _lastDoorMoved + _roomMovingCooldown;
        }
    }
}