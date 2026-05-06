using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class SelectedRoomPattern : ISelectedRoomPattern
    {
        public Rect CellRect { get; }
        public Rect RoomRect { get; }
        public IEnumerable<IPortDefinition> PortDefinitions { get; }
        
        public SelectedRoomPattern(Rect cellRect, Rect roomRect, IEnumerable<IPortDefinition> portDefinitions)
        {
            CellRect = cellRect;
            RoomRect = roomRect;
            PortDefinitions = portDefinitions;
        }
    }
}