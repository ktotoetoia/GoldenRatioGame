using UnityEngine;

namespace IM.Visuals
{
    public class Palette
    {
        public Texture2D Palette16X3 { get; }

        public Palette(Texture2D palette16X3)
        {
            Palette16X3 = palette16X3;
            
            if (palette16X3.width != 16 || palette16X3.height != 3)
            {
                Debug.LogWarning($"[Palette] Invalid texture dimensions: {palette16X3.width}x{palette16X3.height}. Expected 16x3. The texture will be cropped to fit.");
                palette16X3.width = 16;
                palette16X3.height = 3;
            }
        }
    }
}