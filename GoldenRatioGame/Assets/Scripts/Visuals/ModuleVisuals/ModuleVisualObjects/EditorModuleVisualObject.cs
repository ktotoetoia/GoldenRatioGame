using UnityEngine;

namespace IM.Visuals
{
    public class EditorModuleVisualObject : ModuleVisualObject, IEditorModuleVisualObject
    {
        [SerializeField] private BoundsSource _boundsSource = BoundsSource.Renderer;
        
        private readonly Bounds _defaultEditorLocalBounds = new (Vector3.zero, Vector3.one);

        public Bounds DefaultEditorLocalBounds
        {
            get
            {
                switch (_boundsSource)
                {
                    case BoundsSource.Renderer:
                        return _renderer.localBounds;
                    default:
                        return _defaultEditorLocalBounds;
                }
            }
        }

        public Bounds EditorBounds
        {
            get
            {
                switch (_boundsSource)
                {
                    case BoundsSource.Renderer:
                        return _renderer.bounds;
                    default:
                        return new(transform.position, _defaultEditorLocalBounds.size);
                }
            }
        }
        
        private enum BoundsSource
        {
            None = 0,
            Renderer = 1,
        }
    }
}