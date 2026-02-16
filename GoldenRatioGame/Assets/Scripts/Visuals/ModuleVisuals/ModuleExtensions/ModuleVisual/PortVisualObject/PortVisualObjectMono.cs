using System;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class PortVisualObjectMono : MonoBehaviour, IPortVisualObject
    {
        [SerializeField] private Renderer _renderer;
        private ITransform _transform;
        private LocalTransformPreset _localTransformPreset;
        private int _sortingOrder;
        private bool _visible;
        
        [field:SerializeField] public int OutputOrderAdjustment { get; set; }
        public bool Visible
        {
            get => gameObject.activeInHierarchy;
            set 
            {
                gameObject.SetActive(value);
                if (_renderer) _renderer.gameObject.SetActive(value);
            }
        }

        public int Order
        {
            get => _sortingOrder;
            set
            {
                _sortingOrder = value;
                if (_renderer) _renderer.sortingOrder = value;
            }
        }

        public int Layer
        {
            get => gameObject.layer;
            set
            {
                if (gameObject.layer == value) return;
                SetLayerRecursively(transform,value);
            }
        }
        public IModuleVisualObject OwnerVisualObject { get; private set; }
        public IPort Port { get; private set; }
        public ITransform Transform => _transform;
        public bool Highlighted { get; set; }

        public LocalTransformPreset LocalTransformPreset
        {
            get=> _localTransformPreset;
            set
            {
                _localTransformPreset = value;
                _localTransformPreset.ApplyTo(Transform);
            }
        }

        private void Awake()
        {
            if (!TryGetComponent(out _transform)) throw new MissingComponentException(nameof(ITransform));
        }

        private void SetLayerRecursively(Transform targetTransform, int layer)
        {
            targetTransform.gameObject.layer = layer;

            for (int i = 0; i < targetTransform.childCount; i++)
            {
                SetLayerRecursively(targetTransform.GetChild(i), layer);
            }
        }

        public void Initialize(IModuleVisualObject ownerVisualObject, IPort port)
        {
            if (OwnerVisualObject != null && Port != null) throw new InvalidOperationException("PortVisualObject is already initialized");
            
            OwnerVisualObject = ownerVisualObject ?? throw new ArgumentNullException(nameof(ownerVisualObject));
            Port = port ??  throw new ArgumentNullException(nameof(port));
        }
        
        public void Reset()
        {
            _localTransformPreset.ApplyTo(Transform);
        }
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
    } 
}