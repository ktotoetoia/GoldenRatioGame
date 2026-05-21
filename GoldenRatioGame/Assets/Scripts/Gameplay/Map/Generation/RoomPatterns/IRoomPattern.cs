using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoomPattern
    {
        IRoomShape Shape { get; }
        IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> RequiredPortDefinitions { get; }
        IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> OptionalPortDefinitions { get; }

        ISelectedRoomPattern Select(IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> selectedOptionalPorts);
    }
}