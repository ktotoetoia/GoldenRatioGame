using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class TriggerRoomWalker : RoomWalkerMono
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IRoomPort roomPort)) return;

            if (!AvailablePorts.Contains(roomPort)) return;
            
            GoTo(roomPort.Destination.Origin);
            transform.position = roomPort.Destination.DeploymentPosition;
        }
    }
}