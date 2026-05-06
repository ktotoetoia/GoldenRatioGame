using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoomPattern
    {
        Rect CellRect { get; }
        Rect RoomRect { get; }
        
        IEnumerable<IPortDefinition> RequiredPortDefinitions { get; }
        IEnumerable<IPortDefinition> OptionalPortDefinitions { get; }
        
        ISelectedRoomPattern Select(IEnumerable<IPortDefinition> selectedOptionalPorts);
    }
}