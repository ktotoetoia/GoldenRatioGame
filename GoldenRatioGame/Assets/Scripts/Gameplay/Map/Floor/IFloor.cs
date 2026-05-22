using System.Collections.Generic;
using IM.Graphs;
using IM.Map.Grid;

namespace IM.Map
{
    public interface IFloor
    {
        IDataGraph<IGameObjectRoom> FloorGraph { get; }
        IEnumerable<IRoomWalker> RoomWalkers { get; }
        
        void SetMapFactory(IMapInfoFactory infoFactory);
        void AddRoomWalker(IRoomWalker walker);
        void RemoveRoomWalker(IRoomWalker walker);
    }
}