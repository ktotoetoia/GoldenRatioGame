using System.Collections.Generic;
using IM.Factions;
using UnityEngine;

namespace IM.Map
{

    public class RoomPortsController : MonoBehaviour
    {
        [SerializeField] private bool _closePortsIfMoreThan2FactionsInTheRoom = true;
        private IRoomEvents _roomEvents;

        private void Awake()
        {
            
        }

        private void Update()
        {
            
        }
    }
}