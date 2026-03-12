namespace IM.Visuals
{
    public interface IPaletteSwapper
    {
        Palette SourcePalette { get; }
        Palette AppliedPalette { get; set; }
    }
}