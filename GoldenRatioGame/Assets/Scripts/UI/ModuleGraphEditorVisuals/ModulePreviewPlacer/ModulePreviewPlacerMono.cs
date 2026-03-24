using System;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class ModulePreviewPlacerMono : MonoBehaviour, IModulePreviewPlacer
    {
        [SerializeField] private Transform _previewParent;
        [SerializeField] private Camera _camera;
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModulePreviewPlacer _modulePreviewPlacer;

        public IModuleVisualObject PreviewObject => _modulePreviewPlacer.PreviewObject;
        public bool IsPreviewing => _modulePreviewPlacer.IsPreviewing;

        public Func<IModuleVisualObject, Vector3> HoverPositionSource { get; set; } = x => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        private void Awake()
        {
            _modulePreviewPlacer = new ModulePreviewPlacer(_previewParent, _preset, GetHoverPosition);
        }
        
        public void StartPreview(IExtensibleModule module)
        {
            _modulePreviewPlacer.StartPreview(module);
        }

        public void UpdatePreviewPosition()
        {
            _modulePreviewPlacer.UpdatePreviewPosition();
        }

        public IExtensibleModule FinalizePreview()
        {
            return _modulePreviewPlacer.FinalizePreview();
        }

        public void StopPreview()
        {
            _modulePreviewPlacer.StopPreview();
        }

        private Vector3  GetHoverPosition(IModuleVisualObject obj)
        {
            return HoverPositionSource(obj);
        }
    }
}