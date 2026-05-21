using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Initialization/Quick Matching Map Generator")]
    public class QuickMatchingMapFactory : MapFactory
    {
        [SerializeField] private RoomFactory _startingRoomFactory;
        [SerializeField] private RoomFactory _filledRoomFactory;
        [SerializeField] private RoomFactory _specialRoomFactory;
        [SerializeField] private RoomFactory _finalRoomFactory;
        [SerializeField] private int _baseRoomCount = 4;
        [SerializeField] private int _roomCountPerDepth = 2;
        [field: SerializeField] public float FilledRoomToSpecialRoomRatio { get; set; } = 4;
        
        public override IMapInfo Create(IGameObjectFactory factory, int seed, int depth)
        {
            int floorSeed = seed + depth;
            
            IGrid<ICellInfo> grid = BuildGrid(_baseRoomCount + depth * _roomCountPerDepth, floorSeed);
            
            IDataGraph<IGameObjectRoom> graph = new RoomGraphFactory(factory).Create(grid);
            return new MapInfo(graph);
        }

        private IGrid<ICellInfo> BuildGrid(int roomCount, int seed)
        {
            int specialRoomCount = (int)(roomCount / FilledRoomToSpecialRoomRatio);
            int filledRoomCount = roomCount - specialRoomCount;
            
            List<(IRoomFactory, int)> s = new()
            {
                (_startingRoomFactory, 1),
                (_filledRoomFactory, filledRoomCount),
                (_specialRoomFactory,specialRoomCount),
                (_finalRoomFactory,1)
            };
            
            IGrid<ICellInfo> grid = new PriorityBasedRandomGridFactory(s,seed).CreateGrid(roomCount,roomCount);
            new MultiCellPatternSelector().SelectMatchingRoomPatterns(grid,seed);
            GridDebugPrinter.PrintGrid(grid);

            return grid;
        }
    }
}