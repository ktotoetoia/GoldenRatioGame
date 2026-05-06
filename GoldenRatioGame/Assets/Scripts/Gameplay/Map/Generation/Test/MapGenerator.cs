using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    [CreateAssetMenu(menuName = "Initialization/Map Generator")]
    public class MapGenerator : ScriptableObject
    {
        [SerializeField] private RoomFactory _startingRoomFactory;
        [SerializeField] private RoomFactory _filledRoomFactory;
        [SerializeField] private RoomFactory _specialRoomFactory;
        [SerializeField] private RoomFactory _finalRoomFactory;
        [SerializeField] private GameObject _floorPrefab;

        public void Create(IGameObjectFactory factory, int roomCount, int seed)
        {
            Random random = new Random(seed);

            RoomGrid grid = BuildGrid(roomCount, random);
            new RoomGridSelector().SelectAll(grid);
            IDataGraph<IGameObjectRoom> graph = new RoomGraphFactory(factory).Create(grid);

            var floor = factory.Create(_floorPrefab, false).GetComponent<Floor>();
            floor.SetFloorGraph(graph);
            UpdateWalkers(graph.DataNodes.FirstOrDefault()?.Value);
        }

        private RoomGrid BuildGrid(int roomCount, Random random)
        {
            RoomGrid grid = new RoomGrid(roomCount, roomCount);
            RoomGridPlacer placer = new RoomGridPlacer(random);

            Vector2Int center = new Vector2Int(roomCount / 2, roomCount / 2);
            placer.Place(grid, _startingRoomFactory, center);

            for (int i = 0; i < roomCount; i++)
                placer.PlaceClose(grid, _filledRoomFactory);

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