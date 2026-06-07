using System;
using IM.Transforms;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class PreviewPlacer<TObject, TVisual> : IPreviewPlacer<TObject,TVisual> where TVisual : IVisualObject, IHaveTransform
    {
        private readonly Func<TVisual, Vector3> _getPreviewPosition;
        private readonly Func<TObject, TVisual> _getVisual;
        private readonly Action<TObject, TVisual> _releaseVisual;

        public TVisual PreviewVisual { get; private set; }
        public TObject PreviewObject { get; private set; }

        public bool IsPreviewing => PreviewVisual != null;
        public Func<TVisual, Vector3> GetOffset { get; set; } = x =>  Vector3.zero;
        
        public PreviewPlacer(Func<TVisual, Vector3> getPreviewPosition,Func<TObject, TVisual> getVisual,Action<TObject,TVisual> releaseVisual)
        {
            _getPreviewPosition = getPreviewPosition;
            _getVisual = getVisual;
            _releaseVisual = releaseVisual;
        }
        
        public void StartPreview(TObject module)
        {
            if (PreviewVisual != null) StopPreview();
            
            PreviewObject = module;
            PreviewVisual = _getVisual(module);
            
            if (PreviewVisual == null) return;
            
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (PreviewVisual == null) return;
            
            Vector3 offset = GetOffset(PreviewVisual);
            
            PreviewVisual.Transform.Position = _getPreviewPosition(PreviewVisual) - offset;
        }

        public TObject FinalizePreview()
        {
            if (PreviewVisual == null) return default;
            
            TObject result = PreviewObject;
            
            StopPreview();
            
            return result;
        }

        public void StopPreview()
        {
            if (PreviewVisual == null) return;
            
            _releaseVisual?.Invoke(PreviewObject, PreviewVisual);
            PreviewVisual = default;
            PreviewObject = default;
        }
    }
}