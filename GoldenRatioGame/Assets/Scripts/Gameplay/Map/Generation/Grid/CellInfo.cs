using UnityEngine;

namespace IM.Map.Grid
{
    public class CellInfo : ICellInfo
    {
        public IRoomFactory Factory { get; }
        public IRoomPattern Pattern { get; set; }
        public Vector2Int Offset { get; set; }
        public int RoomInstanceId { get; set; }
        public ISelectedRoomPattern SelectedPattern { get; set; }

        public CellInfo(IRoomFactory factory)
        {
            Factory = factory;
        }
        
        public CellInfo(IRoomPattern pattern, IRoomFactory factory)
        {
            Pattern = pattern;
            Factory = factory;
        }
    }
}