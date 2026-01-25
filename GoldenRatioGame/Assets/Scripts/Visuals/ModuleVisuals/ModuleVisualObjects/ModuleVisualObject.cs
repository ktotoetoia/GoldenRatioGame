using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObject : MonoBehaviour, IAnimatedModuleVisualObject
    {
        [SerializeField] bool _isVisibleOnAwake;
        private readonly List<IPortVisualObject> _portVisuals = new();
        private readonly HierarchyTransform _transform = new();
        private Animator _animator;
        
        public IHierarchyTransform Transform => _transform;
        public IReadOnlyList<IPortVisualObject> PortsVisuals => _portVisuals;
        public IExtensibleModule Owner { get; set; }
        public bool IsAnimating { get; set; } = true;
        public IEnumerable<IAnimationChange> AnimationChanges { get; set; }
        public IModuleGraphStructureUpdater ModuleGraphStructureUpdater { get; set; }

        public bool Visibility
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Visibility = _isVisibleOnAwake;
            _transform.PositionChanged += (_, newValue) => transform.position = newValue;
            _transform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _transform.RotationChanged += (_, newValue) => transform.rotation = newValue;
        }

        private void Update()
        {
            if (AnimationChanges == null || !IsAnimating) return;
         
            foreach (IAnimationChange animationChange in AnimationChanges)
            {
                animationChange.ApplyToAnimator(_animator);
            }
        }
        
        public void AddPort(IPortVisualObject port, IHierarchyTransform hierarchyTransform)
        {
            _portVisuals.Add(port);
            _transform.AddChildKeepLocal(hierarchyTransform);
        }
        
        public IPortVisualObject GetPortVisual(IPort port)
        {
            return _portVisuals.FirstOrDefault(x => x.Port == port);
        }

        public void Dispose()
        {
            _transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}