using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.ModuleGraph;
using IM.Values;
using UnityEngine;
using Transform = IM.ModuleGraph.Transform;

namespace IM.Modules
{
    public class ModuleGraphVisual : MonoBehaviour,IModuleGraphVisual
    {
        private readonly Dictionary<Texture, Material> _materialCache = new();
        private readonly Dictionary<IPort, IVisualPort> _visualPortMap = new();
        private IVisualCommandModuleGraph _visualGraph;
        
        public IModuleGraphReadOnly Source { get; private set; }
        
        private void OnDrawGizmos()
        {
            if (_visualGraph == null) return;

            foreach (IVisualModule module in _visualGraph.Modules)
            {
                Bounds localBounds = BoundsUtility.CreateBoundsNormalized(module.Ports.Select(p => p.Transform.LocalPosition));

                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(module.Transform.Position, module.Transform.Rotation, Vector3.one);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(Vector3.zero, localBounds.size);
                Gizmos.matrix = oldMatrix;

                Vector3 center = module.Transform.Position;
                Vector3 size = localBounds.size;
                Sprite sprite = module.Sprite;
                
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
                
                foreach (IVisualPort port in module.Ports)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(port.Transform.Position, 0.1f);
                }
            }

            Gizmos.color = Color.cyan;
            foreach (IVisualConnection connection in _visualGraph.Connections)
            {
                Vector3 from = connection.Output.Transform.Position;
                Vector3 to = connection.Input.Transform.Position;
                Gizmos.DrawLine(from, to);
            }
        }

        private Material GetMaterialForSprite(Sprite s)
        {
            if (s == null) return null;
            var tex = s.texture;
            if (!_materialCache.TryGetValue(tex, out var mat))
            {
                var shader = Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Texture");
                mat = new Material(shader) { mainTexture = tex, hideFlags = HideFlags.HideAndDontSave };
                _materialCache[tex] = mat;
            }

            Rect tr = s.textureRect;
            var texSize = new Vector2(tex.width, tex.height);
            mat.SetTextureScale("_MainTex", new Vector2(tr.width / texSize.x, tr.height / texSize.y));
            mat.SetTextureOffset("_MainTex", new Vector2(tr.x / texSize.x, tr.y / texSize.y));
            return mat;
        }
        
        public void SetSource(IModuleGraphReadOnly source, ICoreGameModule coreModule)
        {
            Source = source;
            _visualGraph = new VisualCommandModuleGraph();
            _visualPortMap.Clear();
            
            var traversal = new BreadthFirstTraversal();
            var moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = Create(module.ModuleLayout);
                moduleToVisual[module] = visualModule;
                _visualGraph.AddModule(visualModule);
                visualModule.Sprite =  module.ModuleLayout.Sprite;
            }

            foreach (IConnection connection in Source.Connections)
            {
                if (connection.Input?.Module is not IGameModule inputModule ||
                    connection.Output?.Module is not IGameModule outputModule)
                    continue;

                if (!moduleToVisual.TryGetValue(inputModule, out IVisualModule inputVisual) ||
                    !moduleToVisual.TryGetValue(outputModule, out IVisualModule outputVisual))
                    continue;

                if (!_visualPortMap.TryGetValue(connection.Input, out IVisualPort inputPort) ||
                    !_visualPortMap.TryGetValue(connection.Output, out IVisualPort outputPort))
                    continue;

                _visualGraph.Connect(outputPort, inputPort);
            }
        }

        private IVisualModule Create(IModuleLayout moduleLayout)
        {
            GizmosVisualModule gizmosVisualModule = new GizmosVisualModule(new Transform(Vector3.zero, Vector3.one, new Quaternion(0,0,0,1)));
            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(
                    gizmosVisualModule,
                    new Transform(gizmosVisualModule.Transform, portLayout.RelativePosition,Vector3.one,  Quaternion.LookRotation(portLayout.Normal, Vector3.up))
                );

                gizmosVisualModule.AddPort(visualPort);
                _visualPortMap[portLayout.Port] = visualPort;
            }

            return gizmosVisualModule;
        }
    }
}