namespace IM.Items
{
    public interface IIconDrawer
    {
        IIcon Icon { get; }
        bool IsDrawing { get; set; }
    }
}