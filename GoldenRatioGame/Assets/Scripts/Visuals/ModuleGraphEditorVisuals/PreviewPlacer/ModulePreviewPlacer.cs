using System;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class ModulePreviewPlacer : MonoBehaviour, IPreviewPlacer<IExtensibleItem,IModuleVisualObject>
    {
        [SerializeField] private Transform _previewParent;
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IPreviewPlacer<IExtensibleItem,IModuleVisualObject> _previewPlacer;

        public IModuleVisualObject PreviewObject => _previewPlacer.PreviewObject;
        public bool IsPreviewing => _previewPlacer.IsPreviewing;
        public Func<IModuleVisualObject, Vector3> HoverPositionSource { get; set; } = x => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        private void Awake()
        {
            _previewPlacer = new PreviewPlacer<IExtensibleItem,IModuleVisualObject>(
                x => HoverPositionSource(x),
                GetVisual,
                (x,y) => x.Extensions.Get<IModuleVisualObjectProvider>().EditorPool.Release(y));
        }

        private IModuleVisualObject GetVisual(IExtensibleItem x)
        {
            var visual =x.Extensions.Get<IModuleVisualObjectProvider>().EditorPool.Get();
            
            visual.Transform.Transform.SetParent(_previewParent,false);
            _preset.ApplyTo(visual);
            
            return visual;
        }

        public void StartPreview(IExtensibleItem module) => _previewPlacer.StartPreview(module);
        public void UpdatePreviewPosition() => _previewPlacer.UpdatePreviewPosition();
        public IExtensibleItem FinalizePreview() => _previewPlacer.FinalizePreview();
        public void StopPreview() => _previewPlacer.StopPreview();
    }
}