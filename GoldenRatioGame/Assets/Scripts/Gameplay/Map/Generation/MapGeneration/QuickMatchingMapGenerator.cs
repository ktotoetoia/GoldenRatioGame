using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    [CreateAssetMenu(menuName = "Initialization/Quick Matching Map Generator")]
    public class QuickMatchingMapGenerator : MapGenerator
    {
        [SerializeField] private RoomFactory _startingRoomFactory;
        [SerializeField] private RoomFactory _filledRoomFactory;
        [SerializeField] private RoomFactory _specialRoomFactory;
        [SerializeField] private RoomFactory _finalRoomFactory;
        [SerializeField] private GameObject _floorPrefab;

        [field: SerializeField] public float FilledRoomToSpecialRoomRatio { get; set; } = 4;
        
        public override void Create(IGameObjectFactory factory, int roomCount, int seed)
        {
            Random random = new Random(seed);

            IGrid<CellInfo> grid = BuildGrid(roomCount, random);
            new RoomGridSelector().SelectAll(grid);
            IDataGraph<IGameObjectRoom> graph = new RoomGraphFactory(factory).Create(grid);

            var floor = factory.Create(_floorPrefab, false).GetComponent<Floor>();
            floor.SetFloorGraph(graph);
            UpdateWalkers(graph.DataNodes.FirstOrDefault(x => x.Value is IHaveRoomType roomType && roomType.RoomType == RoomType.Start)?.Value ?? graph.DataNodes.FirstOrDefault()?.Value);
        }

        private IGrid<CellInfo> BuildGrid(int roomCount, Random random)
        {
            IGrid<CellInfo> grid = new Grid<CellInfo>(roomCount, roomCount);
            RoomGridPlacer placer = new RoomGridPlacer(random);

            Vector2Int center = new Vector2Int(roomCount / 2, roomCount / 2);
            placer.Place(grid, _startingRoomFactory, center);
            
            int specialRoomCount = (int)(roomCount / FilledRoomToSpecialRoomRatio);
            int filledRoomCount = roomCount - specialRoomCount;
            
            for (int i = 0; i < filledRoomCount; i++) placer.PlaceClose(grid, _filledRoomFactory);
            for (int i = 0; i < specialRoomCount; i++) placer.PlaceClose(grid, _specialRoomFactory);

            placer.PlaceClose(grid, _finalRoomFactory);

            return grid;
        }

        private void UpdateWalkers(IGameObjectRoom startRoom)
        {
            if (startRoom == null) return;
            foreach (var walker in FindObjectsByType<RoomWalkerMono>())
                walker.GoTo(startRoom);
        }
    }
}