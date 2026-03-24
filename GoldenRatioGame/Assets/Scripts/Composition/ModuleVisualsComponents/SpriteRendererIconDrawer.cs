using IM.Visuals;
using UnityEngine;

namespace IM.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererIconDrawer : MonoBehaviour, IIconDrawer, IDepthOrderable
    {
        [SerializeField] private Vector2 _referencePositionNormalized = new Vector3(0.5f, 0.5f);
        private SpriteRenderer _renderer;
        private IIcon _icon;

        private SpriteRenderer Renderer => _renderer ??= GetComponent<SpriteRenderer>();
        
        public Vector3 ReferencePoint
        {
            get
            {
                var bounds = Renderer.bounds;

                return new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, _referencePositionNormalized.x),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, _referencePositionNormalized.y),
                    bounds.center.z
                );
            }
        }
        
        public int Order
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }
        
        public IIcon Icon
        {
            get
            {
                _icon ??= new Icon(Renderer.sprite);
                
                return _icon;
            }
        }

        public bool IsDrawing
        {
            get => Renderer.enabled;
            set => Renderer.enabled = value;
        }
    }
}