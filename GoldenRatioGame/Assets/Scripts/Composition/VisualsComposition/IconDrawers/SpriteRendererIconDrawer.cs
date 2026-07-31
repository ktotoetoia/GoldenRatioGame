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

        [field:SerializeField]  public float Elevation { get; private set; }
        [field:SerializeField]  public float Height { get;  private set;}
        [field:SerializeField]  public float HalfWidth { get;  private set;}
        
        public int Order
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }
        
        public IIcon Icon => _icon??= new Icon(Renderer.sprite);

        public bool IsDrawing
        {
            get => Renderer.enabled;
            set => Renderer.enabled = value;
        }
    }
}