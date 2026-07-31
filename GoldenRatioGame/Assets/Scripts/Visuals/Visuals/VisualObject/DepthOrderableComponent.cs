using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(Renderer))]
    public class DepthOrderableComponent : MonoBehaviour, IDepthOrderable
    {
        [Header("Sources")]
        [SerializeField] private ValueSource _referencePointSource = ValueSource.Renderer;
        [SerializeField] private ValueSource _heightSource = ValueSource.Renderer;
        [SerializeField] private ValueSource _halfWidthSource = ValueSource.Renderer;

        [Header("Manual Values")]
        [SerializeField] private Vector3 _manualReferencePoint;
        [SerializeField] private float _manualHeight;
        [SerializeField] private float _manualHalfWidth;

        [Header("Offsets")]
        [SerializeField] private Vector3 _offset;
        
        [field:SerializeField] public float Elevation { get; set; }
        
        private Renderer _renderer;
        private Collider2D _collider;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider2D>();
        }

        public Vector3 ReferencePoint
        {
            get
            {
                Vector3 basePoint = _referencePointSource switch
                {
                    ValueSource.Renderer => _renderer != null ? _renderer.bounds.min : transform.position,
                    ValueSource.Collider => _collider != null ? _collider.bounds.min : transform.position,
                    ValueSource.Manual => _manualReferencePoint,
                    _ => transform.position
                };
                
                return basePoint + _offset;
            }
        }

        public float Height => _heightSource switch
        {
            ValueSource.Renderer => _renderer != null ? _renderer.bounds.size.y : 0f,
            ValueSource.Collider => _collider != null ? _collider.bounds.size.y : 0f,
            ValueSource.Manual => _manualHeight,
            _ => 0f
        };

        public float HalfWidth => _halfWidthSource switch
        {
            ValueSource.Renderer => _renderer != null ? _renderer.bounds.extents.x : 0f,
            ValueSource.Collider => _collider != null ? _collider.bounds.extents.x : 0f,
            ValueSource.Manual => _manualHalfWidth,
            _ => 0f
        };

        public int Order
        {
            get => _renderer != null ? _renderer.sortingOrder : 0;
            set
            {
                if (_renderer != null)
                {
                    _renderer.sortingOrder = value;
                }
            }
        }
    }

    public enum ValueSource
    {
        Manual, 
        Renderer,
        Collider,
    }
}