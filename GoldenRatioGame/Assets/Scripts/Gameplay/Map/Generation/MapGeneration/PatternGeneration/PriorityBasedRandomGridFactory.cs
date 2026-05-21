using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    public class PriorityBasedRandomGridFactory : ICellGridFactory
    {
        private readonly IEnumerable<(IRoomFactory, int)> _factories;
        private readonly Random _random;

        public PriorityBasedRandomGridFactory(IEnumerable<(IRoomFactory, int)> factories,int seed)
        {
            _random = new Random(seed);
            _factories = factories;
        }

        public IGrid<ICellInfo> CreateGrid(int height, int width)
        {
            Grid<ICellInfo> grid = new (height,width);
            bool firstCell = true;
            
            foreach ((IRoomFactory factory, int maxCount) in _factories)
            {
                int count = maxCount;
                
                if (firstCell)
                {
                    grid[height / 2, width / 2] = new CellInfo(factory);

                    count--;
                    firstCell = false;
                }

                for (int i = 0; i < count; i++)
                {
                    List<Vector2Int> availablePositions = GridUtility.GetNearbyUnoccupiedPosition(grid);
                    Vector2Int position = availablePositions[_random.Next(availablePositions.Count)];
                    grid[position] =  new CellInfo(factory);
                }
            }
            
            return grid;
        }
    }
}