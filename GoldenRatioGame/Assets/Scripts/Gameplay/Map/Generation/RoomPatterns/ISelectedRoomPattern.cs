using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface ISelectedRoomPattern
    {
        Rect CellRect { get; }
        Rect RoomRect { get; }
        IEnumerable<IPortDefinition> PortDefinitions { get; }
    }
}