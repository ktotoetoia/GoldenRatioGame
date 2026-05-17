using IM.Graphs;

namespace IM.Map.Grid
{
    public class MapInfo : IMapInfo
    {
        public IDataGraph<IGameObjectRoom> Graph { get; }
        
        public MapInfo(IDataGraph<IGameObjectRoom> graph)
        {
            Graph = graph;
        }
    }
}