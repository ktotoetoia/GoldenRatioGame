using System.Collections.Generic;
using System.Linq;
using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Generation Steps/Priority Based Grid Creation Step")]
    public class PriorityBasedGridCreationStep : MapGenerationStep
    {
        [SerializeField] private RoomFactory _startingRoomFactory;
        [SerializeField] private RoomFactory _filledRoomFactory;
        [SerializeField] private RoomFactory _specialRoomFactory;
        [SerializeField] private RoomFactory _finalRoomFactory;
        [SerializeField] private int _baseRoomCount = 4;
        [SerializeField] private int _roomCountPerDepth = 2;
        [SerializeField] public float _filledRoomToSpecialRoomRatio  = 4;
        
        public override void Execute(MapGenerationContext context)
        {
            if (context.Grid != null && context.Grid.OccupiedPositions().Any())
            {
                Debug.LogWarning("Grid was already created and filled by other step");
                return;
            }

            int roomCount = _baseRoomCount + context.Depth * _roomCountPerDepth;
            int specialRoomCount = (int)(roomCount / _filledRoomToSpecialRoomRatio);
            int filledRoomCount = roomCount - specialRoomCount;
            
            List<(IRoomFactory, int)> s = new()
            {
                (_startingRoomFactory, 1),
                (_filledRoomFactory, filledRoomCount),
                (_specialRoomFactory,specialRoomCount),
                (_finalRoomFactory,1)
            };
            
            context.Grid = new PriorityBasedRandomGridFactory(s, context.Seed).CreateGrid(Vector2Int.one * roomCount,Vector2Int.one * roomCount/2);
        }
    }
}