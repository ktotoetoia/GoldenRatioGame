namespace IM.Map.Grid
{
    public interface ICellGridFactory
    {
        IGrid<ICellInfo> CreateGrid(int height, int width);
    }
}