using UnityEngine;

namespace IM.Map.Grid
{
    public interface ICellInfo
    {
        IRoomFactory Factory { get; }
        IRoomPattern Pattern { get; set; }
        Vector2Int Offset { get; set; }
        int RoomInstanceId { get; set; }
        ISelectedRoomPattern SelectedPattern { get; set; }
    }
}