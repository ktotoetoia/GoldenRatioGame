using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModulePreviewPlacer : IModulePreviewPlacer
    {
        private readonly Transform _parent;
        private readonly IModuleVisualObjectPreset _preset;
        private readonly Func<IModuleVisualObject, Vector3> _getPreviewPosition;
        private IModuleVisual _currentModuleVisual;

        public IModuleVisualObject PreviewObject { get; private set; }
        public bool IsPreviewing => PreviewObject != null;

        public ModulePreviewPlacer(Transform parent, IModuleVisualObjectPreset preset,Func<IModuleVisualObject, Vector3> getPreviewPosition)
        {
            _parent = parent;
            _preset = preset;
            _getPreviewPosition = getPreviewPosition;
        }
        
        public void StartPreview(IExtensibleModule module)
        {
            if (PreviewObject != null) StopPreview();
            if (!module.Extensions.TryGet(out _currentModuleVisual)) return;
            
            PreviewObject = _currentModuleVisual.EditorPool.Get();
            PreviewObject.Transform.Transform.SetParent(_parent, false);
            _preset.ApplyTo(PreviewObject);
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (PreviewObject == null) return;
            
            Vector3 offset = (PreviewObject as IEditorModuleVisualObject)?.DefaultEditorLocalBounds.center ?? default; 
            PreviewObject.Transform.Position = _getPreviewPosition(PreviewObject) - offset;
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