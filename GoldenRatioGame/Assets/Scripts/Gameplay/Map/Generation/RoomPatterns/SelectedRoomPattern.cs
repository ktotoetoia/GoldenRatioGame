using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class SelectedRoomPattern : ISelectedRoomPattern
    {
        public IRoomShape Shape { get; }
        public IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> PortDefinitions { get; }
        
        public SelectedRoomPattern(IRoomShape shape, IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> portDefinitions)
        {
            Shape = shape;
            PortDefinitions = portDefinitions;
        }
    }
}