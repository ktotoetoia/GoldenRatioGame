using UnityEngine;

namespace IM.Map
{
    public class TypedGameObjectRoom : GameObjectRoomMono, IHaveRoomType
    {
        [field:SerializeField] public RoomType RoomType { get; set; }
    }
}