using System.Collections.Generic;
using System.Linq;
using IM.Values;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisualDrawer : IModuleGraphVisualDrawer
    {
        public bool DrawBounds { get; set; }= true;
        public bool DrawSprites { get; set; }= true;
        public bool DrawPorts { get; set; }= true;
        
        private readonly Dictionary<Texture, Material> _materialCache = new();
        
        public void Draw(IVisualModuleGraph source)
        {
            if (source == null) return;

            foreach (IVisualModule module in source.Modules)
            {
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(module.Transform.Position, module.Transform.Rotation, Vector3.one);
                Gizmos.color = Color.white;
                if (DrawBounds) Gizmos.DrawWireCube(Vector3.zero, module.Transform.Scale);
                Gizmos.matrix = oldMatrix;

                Vector3 center = module.Transform.Position;
                Vector3 size = module.Transform.Scale;
                Sprite sprite = module.Sprite;
                
                if (sprite && sprite.texture && DrawSprites)
                {
                    Material mat = GetMaterialForSprite(sprite);
                    mat.SetPass(0);

                    Vector3 right = module.Transform.Rotation * Vector3.right * size.x * 0.5f;
                    Vector3 up = module.Transform.Rotation * Vector3.up    * size.y * 0.5f;

                    GL.Begin(GL.QUADS);
                    GL.Color(Color.white);

                    GL.TexCoord2(0, 0); GL.Vertex(center - right - up);
                    GL.TexCoord2(1, 0); GL.Vertex(center + right - up);
                    GL.TexCoord2(1, 1); GL.Vertex(center + right + up);
                    GL.TexCoord2(0, 1); GL.Vertex(center - right + up);

                    GL.End();
                }

                if (DrawPorts)
                {
                    foreach (IVisualPort port in module.Ports)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(port.Transform.Position, 0.1f);
                    }
                }
            }

            Gizmos.color = Color.cyan;
            foreach (IVisualConnection connection in source.Connections)
            {
                Vector3 from = connection.Output.Transform.Position;
                Vector3 to = connection.Input.Transform.Position;
                Gizmos.DrawLine(from, to);
            }
        }

        private Material GetMaterialForSprite(Sprite s)
        {
            if (s == null) return null;
            Texture2D tex = s.texture;
            if (!_materialCache.TryGetValue(tex, out Material mat))
            {
                Shader shader = Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Texture");
                mat = new Material(shader) { mainTexture = tex, hideFlags = HideFlags.HideAndDontSave };
                _materialCache[tex] = mat;
            }

            Rect tr = s.textureRect;
            Vector2 texSize = new Vector2(tex.width, tex.height);
            mat.SetTextureScale("_MainTex", new Vector2(tr.width / texSize.x, tr.height / texSize.y));
            mat.SetTextureOffset("_MainTex", new Vector2(tr.x / texSize.x, tr.y / texSize.y));
            return mat;
        }
    }
}