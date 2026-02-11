using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModulePreviewPlacer : IModulePreviewPlacer
    {
        private readonly Camera _camera;
        private readonly Transform _toAdd;
        private readonly ModuleVisualObjectPreset _preset;
        private IModuleVisual _currentModuleVisual;
        private IModuleVisualObject _previewObject;
        
        public bool IsPreviewing => _previewObject != null;

        public ModulePreviewPlacer(Camera camera, Transform toAdd, ModuleVisualObjectPreset preset)
        {
            _camera = camera;
            _toAdd = toAdd;
            _preset = preset;
        }
        
        public void StartPreview(IExtensibleModule module)
        {
            if (_previewObject != null) StopPreview();
            if (!module.Extensions.TryGetExtension(out _currentModuleVisual)) return;
            
            _previewObject = _currentModuleVisual.EditorPool.Get();
            _previewObject.Transform.Transform.SetParent(_toAdd, false);
            _preset.ApplyTo(_previewObject);
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (_previewObject == null) return;
            Vector3 p = _camera.ScreenToWorldPoint(Input.mousePosition);
            p.Scale(new Vector3(1, 1, 0));
            _previewObject.Transform.Position = p;
        }

        public IExtensibleModule FinalizePreview()
        {
            if (_previewObject == null) throw new InvalidOperationException();
            
            IExtensibleModule result = _previewObject.Owner;
            StopPreview();
            return result;
        }

        public void StopPreview()
        {
            if (_previewObject == null) return;
            
            _currentModuleVisual.EditorPool.Release(_previewObject);
            _previewObject = null;
            _currentModuleVisual = null;
        }
    }
}