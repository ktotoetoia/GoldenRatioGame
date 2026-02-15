using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModulePreviewPlacerMono : MonoBehaviour, IModulePreviewPlacer
    {
        [SerializeField] private Transform _previewParent;
        [SerializeField] private Camera _camera;
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModulePreviewPlacer _modulePreviewPlacer;

        public IModuleVisualObject PreviewObject => _modulePreviewPlacer.PreviewObject;
        public bool IsPreviewing => _modulePreviewPlacer.IsPreviewing;
        
        private void Awake()
        {
            _modulePreviewPlacer = new ModulePreviewPlacer(_camera,_previewParent,_preset);
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
    }
}