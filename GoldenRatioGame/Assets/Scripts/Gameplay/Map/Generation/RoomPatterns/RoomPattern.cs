using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class RoomPattern : IRoomPattern
    {
        public Rect CellRect { get; }
        public Rect RoomRect { get; }
        
        public IEnumerable<IPortDefinition> RequiredPortDefinitions { get; }
        public IEnumerable<IPortDefinition> OptionalPortDefinitions { get; }
        
        public RoomPattern(Rect cellRect, Rect roomRect, IEnumerable<IPortDefinition> requiredPortDefinitions) : this(cellRect,roomRect, requiredPortDefinitions, new List<IPortDefinition>())
        {
            
        }
        
        public RoomPattern(Rect cellRect, Rect roomRect,IEnumerable<IPortDefinition> requiredPortDefinitions,IEnumerable<IPortDefinition> optionalPortDefinitions)
        {
            if (roomRect.xMin < cellRect.xMin ||
             roomRect.yMin < cellRect.yMin ||
             roomRect.xMax > cellRect.xMax ||
             roomRect.yMax > cellRect.yMax)
            {
                throw new ArgumentException(
                    $"RoomRect {roomRect} must be contained within ReservedRect {cellRect}");
            }
            
            CellRect = cellRect;
            RoomRect = roomRect;
            RequiredPortDefinitions = requiredPortDefinitions;
            OptionalPortDefinitions = optionalPortDefinitions;
        }
        
        public ISelectedRoomPattern Select(IEnumerable<IPortDefinition> selectedOptionalPorts)
        {
            return new SelectedRoomPattern(CellRect,RoomRect,RequiredPortDefinitions.Concat(selectedOptionalPorts ?? new List<IPortDefinition>()));
        }
    }
}