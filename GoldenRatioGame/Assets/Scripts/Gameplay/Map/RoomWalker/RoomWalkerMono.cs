using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class RoomWalkerMono : MonoBehaviour, IRoomWalker
    {
        [SerializeField] private float _roomMovingCooldown = 1f;
        private float _lastDoorMoved = float.MinValue;

        public IGameObjectRoom Current { get; private set; }

        public IEnumerable<IRoomPort> AvailablePorts
        {
            get
            {
                if (Current == null || !CooldownFinished()) 
                    return Enumerable.Empty<IRoomPort>();

                return Current.RoomPorts.Where(port => port.IsOpen);
            }
        }

        public IEnumerable<IGameObjectRoom> Available => AvailablePorts.Select(p => p.Connection.Origin);

        public void GoTo(IGameObjectRoom room)
        {
            if (Current != null && !CanMoveTo(room)) return;
            
            if(Current != null) _lastDoorMoved = Time.time;
            
            Current?.Remove(gameObject);
            Current = room;
            Current.Add(gameObject);
        }

        private bool CanMoveTo(IGameObjectRoom room)
        {
            return Available.Any(r => r == room);
        }

        private bool CooldownFinished()
        {
            return Time.time >= _lastDoorMoved + _roomMovingCooldown;
        }
    }
}