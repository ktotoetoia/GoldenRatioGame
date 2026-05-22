using IM.Map.Grid;

namespace IM.Map
{
    public class MapGenerationContext
    {
        public IGrid<ICellInfo> Grid { get; set; }
        public int Seed { get; set; }
        public int Depth { get; set; }
        
        public MapGenerationContext(int seed, int depth)
        {
            Seed = seed;
            Depth = depth;
        }
    }
}