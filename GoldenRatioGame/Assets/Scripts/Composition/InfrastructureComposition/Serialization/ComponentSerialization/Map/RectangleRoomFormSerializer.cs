using System;
using System.Collections.Generic;
using System.Linq;
using IM.Map;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    public class RectangleRoomFormSerializer : ComponentSerializer<RectangleRoomForm>
    {
        [Serializable]
        public class RectangleRoomFormData
        {
            public Rect rect;
            public List<Vector2Int> offsets;
        }

        public override object CaptureState(RectangleRoomForm component)
        {
            if (component == null || component.RoomShape == null) return null;

            return new RectangleRoomFormData
            {
                rect = component.RoomShape.CellRect,
                offsets = component.RoomShape.Offsets?.ToList()
            };
        }

        public override void RestoreState(RectangleRoomForm component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not RectangleRoomFormData data || component == null) return;

            var structuralOffsets = data.offsets is { Count: > 0 } 
                ? data.offsets.ToHashSet() 
                : new HashSet<Vector2Int> { Vector2Int.zero };

            var restoredShape = new RoomShape(data.rect, structuralOffsets);
            component.Apply(restoredShape);
        }
    }
}