namespace IM.Map.Grid
{
    public interface IPatternSelector
    {
        void SelectMatchingRoomPatterns(IGrid<ICellInfo> grid, int seed);
    }
}