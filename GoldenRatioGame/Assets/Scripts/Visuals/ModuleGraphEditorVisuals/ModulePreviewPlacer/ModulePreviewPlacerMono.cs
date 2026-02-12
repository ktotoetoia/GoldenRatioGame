using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModulePreviewPlacerMono : MonoBehaviour, IModulePreviewPlacer
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private ModuleVisualObjectPreset _preset;
        
        private IModulePreviewPlacer _modulePreviewPlacer;

        private void Awake()
        {
            _modulePreviewPlacer = new ModulePreviewPlacer(_camera,transform,_preset);
        }

        public bool IsPreviewing => _modulePreviewPlacer.IsPreviewing;

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