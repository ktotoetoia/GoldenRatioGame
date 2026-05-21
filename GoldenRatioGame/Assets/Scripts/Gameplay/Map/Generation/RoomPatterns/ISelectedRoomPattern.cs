using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface ISelectedRoomPattern
    {
        IRoomShape Shape { get; }
        IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> PortDefinitions { get; }
    }
}