using System;
using IM.Visuals;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.UI
{
    public class WeaponPreviewPlacer : MonoBehaviour, IPreviewPlacer<IWeapon,IWeaponVisual>
    {
        [SerializeField] private Transform _previewParent;
        [SerializeField] private VisualObjectPreset _preset;
        private IPreviewPlacer<IWeapon, IWeaponVisual> _previewPlacer;

        public IWeaponVisual PreviewObject => _previewPlacer.PreviewObject;
        public bool IsPreviewing => _previewPlacer.IsPreviewing;
        public Func<IWeaponVisual, Vector3> HoverPositionSource { get; set; } = x => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            _previewPlacer = new PreviewPlacer<IWeapon, IWeaponVisual>(
                    x => HoverPositionSource(x),
                    GetVisual, 
                    (x,y) => x.WeaponVisualsProvider.WeaponVisualsPool.Release(y));
        }

        private IWeaponVisual GetVisual(IWeapon arg)
        {
            IWeaponVisual visual = arg.WeaponVisualsProvider.WeaponVisualsPool.Get();
            
            visual.Transform.Transform.SetParent(_previewParent,false);
            _preset.ApplyTo(visual);
            
            return visual;
        }

        public void StartPreview(IWeapon module) =>  _previewPlacer.StartPreview(module);
        public void UpdatePreviewPosition() => _previewPlacer.UpdatePreviewPosition();
        public IWeapon FinalizePreview() => _previewPlacer.FinalizePreview();
        public void StopPreview() =>  _previewPlacer.StopPreview();
    }
}