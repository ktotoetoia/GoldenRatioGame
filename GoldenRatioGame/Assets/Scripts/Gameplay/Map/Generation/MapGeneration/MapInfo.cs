using System.Linq;
using IM.Graphs;

namespace IM.Map.Grid
{
    public class MapInfo : IMapInfo
    {
        public IDataGraph<IGameObjectRoom> Graph { get; }
        public IGameObjectRoom StartRoom { get; }
        public IGameObjectRoom FinalRoom { get; }

        public MapInfo(IDataGraph<IGameObjectRoom> graph) : this(graph,
            graph.DataNodes.FirstOrDefault(x => x.Value is IHaveRoomType { RoomType: RoomType.Start })?.Value ?? graph.DataNodes.First().Value,
            graph.DataNodes.FirstOrDefault(x => x.Value is IHaveRoomType { RoomType: RoomType.Final })?.Value ?? graph.DataNodes.Last().Value)
        {
            
        }
        
        public MapInfo(IDataGraph<IGameObjectRoom> graph,  IGameObjectRoom startRoom, IGameObjectRoom finalRoom)
        {
            Graph = graph;
            StartRoom = startRoom;
            FinalRoom = finalRoom;
        }
    }
}