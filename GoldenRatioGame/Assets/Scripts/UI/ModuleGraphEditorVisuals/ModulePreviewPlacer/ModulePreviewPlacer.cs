using System;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class ModulePreviewPlacer : IModulePreviewPlacer
    {
        private readonly Transform _parent;
        private readonly IModuleVisualObjectPreset _preset;
        private readonly Func<IModuleVisualObject, Vector3> _getPreviewPosition;
        private IModuleVisualObjectProvider _currentModuleVisualObjectProvider;

        public IModuleVisualObject PreviewObject { get; private set; }
        public bool IsPreviewing => PreviewObject != null;

        public ModulePreviewPlacer(Transform parent, IModuleVisualObjectPreset preset,Func<IModuleVisualObject, Vector3> getPreviewPosition)
        {
            _parent = parent;
            _preset = preset;
            _getPreviewPosition = getPreviewPosition;
        }
        
        public void StartPreview(IExtensibleItem module)
        {
            if (PreviewObject != null) StopPreview();
            if (!module.Extensions.TryGet(out _currentModuleVisualObjectProvider)) return;
            
            PreviewObject = _currentModuleVisualObjectProvider.EditorPool.Get();
            PreviewObject.Transform.Transform.SetParent(_parent, false);
            _preset.ApplyTo(PreviewObject);
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (PreviewObject == null) return;
            
            Vector3 offset = PreviewObject.LocalBounds.center; 
            PreviewObject.Transform.Position = _getPreviewPosition(PreviewObject) - offset;
        }

        public IExtensibleItem FinalizePreview()
        {
            if (PreviewObject == null) throw new InvalidOperationException();
            
            IExtensibleItem result = PreviewObject.Owner;
            StopPreview();
            return result;
        }

        public void StopPreview()
        {
            if (PreviewObject == null) return;
            
            _currentModuleVisualObjectProvider.EditorPool.Release(PreviewObject);
            PreviewObject = null;
            _currentModuleVisualObjectProvider = null;
        }
    }
}