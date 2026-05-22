using IM.Graphs;
using IM.LifeCycle;
using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Default Map Factory")]
    public class DefaultMapInfoFactory : MapInfoFactory
    {
        [SerializeField] private MapGenerator _mapGenerator; 
        
        public override IMapInfo Create(IGameObjectFactory factory, int seed, int depth)
        {
            int floorSeed = seed + depth;
            
            MapGenerationContext context = _mapGenerator.Generate(floorSeed, depth);
            
            IDataGraph<IGameObjectRoom> graph = new RoomGraphFactory(factory).Create(context);
         
            return new MapInfo(graph);
        }
    }
}