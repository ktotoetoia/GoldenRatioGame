using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class RoomPattern : IRoomPattern
    {
        public IRoomShape Shape { get; }
        public IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> RequiredPortDefinitions { get; }
        public IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> OptionalPortDefinitions { get; }
        
        public RoomPattern(IRoomShape roomShape, IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> requiredPortDefinitions) :
            this(roomShape, requiredPortDefinitions, new Dictionary<Vector2Int, IEnumerable<IPortDefinition>>())
        {
        }
        
        public RoomPattern(IRoomShape roomShape,
            IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> requiredPortDefinitions,
            IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> optionalPortDefinitions)
        {
            RequiredPortDefinitions = requiredPortDefinitions;
            OptionalPortDefinitions = optionalPortDefinitions;
            Shape =  roomShape;
        }
        
        public ISelectedRoomPattern Select(IReadOnlyDictionary<Vector2Int, IEnumerable<IPortDefinition>> selectedOptionalPorts)
        {
            Dictionary<Vector2Int, IEnumerable<IPortDefinition>> requiredPortDefinitions = new(RequiredPortDefinitions);
            
            foreach (KeyValuePair<Vector2Int, IEnumerable<IPortDefinition>> selectedOptionalPort in selectedOptionalPorts)
            {
                if (requiredPortDefinitions.ContainsKey(selectedOptionalPort.Key))
                {
                    requiredPortDefinitions[selectedOptionalPort.Key] = requiredPortDefinitions[selectedOptionalPort.Key].Concat(selectedOptionalPort.Value);
                }
                else
                {
                    requiredPortDefinitions.Add(selectedOptionalPort.Key, selectedOptionalPort.Value);
                }
            }
            
            return new SelectedRoomPattern(Shape, requiredPortDefinitions);
        }
    }
}