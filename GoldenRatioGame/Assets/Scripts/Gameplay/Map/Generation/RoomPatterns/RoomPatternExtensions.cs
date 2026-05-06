using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public static class RoomPatternExtensions
    {
        public static ISelectedRoomPattern SelectRandom(this IRoomPattern roomPattern)
        {
            return roomPattern.Select(roomPattern.OptionalPortDefinitions);
        }
    }
}