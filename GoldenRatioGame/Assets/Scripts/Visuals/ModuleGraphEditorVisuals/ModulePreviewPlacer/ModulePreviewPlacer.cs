using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModulePreviewPlacer : IModulePreviewPlacer
    {
        private readonly Camera _camera;
        private readonly Transform _parent;
        private readonly IModuleVisualObjectPreset _preset;
        private IModuleVisual _currentModuleVisual;
        
        public IModuleVisualObject PreviewObject { get; private set; }
        public bool IsPreviewing => PreviewObject != null;

        public ModulePreviewPlacer(Camera camera, Transform parent, IModuleVisualObjectPreset preset)
        {
            _camera = camera;
            _parent = parent;
            _preset = preset;
        }
        
        public void StartPreview(IExtensibleModule module)
        {
            if (PreviewObject != null) StopPreview();
            if (!module.Extensions.TryGetExtension(out _currentModuleVisual)) return;
            
            PreviewObject = _currentModuleVisual.EditorPool.Get();
            PreviewObject.Transform.Transform.SetParent(_parent, false);
            _preset.ApplyTo(PreviewObject);
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (PreviewObject == null) return;
            Vector3 p = _camera.ScreenToWorldPoint(Input.mousePosition);
            p.Scale(new Vector3(1, 1, 0));
            PreviewObject.Transform.Position = p;
        }

        public IExtensibleModule FinalizePreview()
        {
            if (PreviewObject == null) throw new InvalidOperationException();
            
            IExtensibleModule result = PreviewObject.Owner;
            StopPreview();
            return result;
        }

        public void StopPreview()
        {
            if (PreviewObject == null) return;
            
            _currentModuleVisual.EditorPool.Release(PreviewObject);
            PreviewObject = null;
            _currentModuleVisual = null;
        }
    }
}