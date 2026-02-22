using UnityEngine;

namespace IM.Visuals
{
    public class PaletteSwapper : IPaletteSwapper
    {
        private static readonly int AppliedPaletteID = Shader.PropertyToID("_PaletteTo");
        private static readonly int SourcePaletteID = Shader.PropertyToID("_SourcePalette");
        private readonly Renderer _renderer;
        private Palette _appliedPalette; 
        
        public Palette SourcePalette { get; }
        public Palette AppliedPalette
        {
            get => _appliedPalette;
            set
            {
                if (!_renderer || _appliedPalette == value) return;

                _renderer.material.SetTexture(AppliedPaletteID, value?.Palette16X3);
                _appliedPalette = value;
            }
        }
        

        public PaletteSwapper(Renderer renderer) : this(renderer, new Palette(renderer.material.GetTexture(SourcePaletteID) as Texture2D))
        {
            
        }

        public PaletteSwapper(Renderer renderer, Palette sourcePalette)
        {
            _renderer = renderer;
            SourcePalette = sourcePalette;
        }
    }
}