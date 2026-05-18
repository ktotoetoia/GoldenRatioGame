using IM.Graphs;

namespace IM.Map.Grid
{
    public interface IMapInfo
    {
        IDataGraph<IGameObjectRoom> Graph { get; } 
        IGameObjectRoom StartRoom { get; }
        IGameObjectRoom FinalRoom { get; }
    }
}