using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public class VisualGraphIconDrawer : IVisualGraphDrawer
    {
        public bool DrawBounds { get; set; } = true;
        public bool DrawSprites { get; set; } = true;
        public bool DrawPorts { get; set; } = true;
        public bool DrawConnections { get; set; } = true;
        
        private readonly Dictionary<Texture, Material> _materialCache = new();
        
        public void Draw(IVisualModuleGraph source)
        {
            if (source == null) return;

            foreach (IVisualModule module in source.Modules)
            {
                if(DrawBounds) DrawGizmosModule(module);
                if(DrawSprites) DrawGLModule(module);
                if (DrawPorts)
                {
                    foreach (IVisualPort port in module.Ports)
                    {
                        DrawGizmosPort(port);
                    }
                }
            }

            if (DrawConnections)
            {
                foreach (IVisualConnection connection in source.Connections)
                {
                    DrawConnection(connection);
                }
            }
        }

        private void DrawConnection(IVisualConnection connection)
        {
            Gizmos.color = Color.cyan;
            Vector3 from = connection.Output.Transform.Position;
            Vector3 to = connection.Input.Transform.Position;
            Gizmos.DrawLine(from, to);
        }

        private void DrawGizmosPort(IVisualPort port)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(port.Transform.Position, 0.1f);
        }

        private void DrawGizmosModule(IVisualModule module)
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(module.Transform.Position, module.Transform.Rotation, Vector3.one);
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, module.Transform.LossyScale);
            Gizmos.matrix = oldMatrix;
        }

        private void DrawGLModule(IVisualModule module)
        {
            Vector3 center = module.Transform.Position;
            Vector3 size = module.Transform.LossyScale;
            Sprite sprite = module.Icon;
                
            if (sprite && sprite.texture)
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