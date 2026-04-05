using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class ProximityRoomWalker : RoomWalkerMono
    {
        [SerializeField] private float _proximityDistance = 0.5f;
        
        private void Update()
        {
            foreach (IRoomPort port in AvailablePorts)
            {
                if (Vector3.Distance(port.Position, transform.position) < _proximityDistance)
                {
                    GoTo(port.Connection.Origin);
                    transform.position = port.Connection.Position;
                }
            }
        }
    }
}