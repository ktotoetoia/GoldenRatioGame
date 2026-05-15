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
        private TObject _result;
        
        public TVisual PreviewObject { get; private set; }
        public bool IsPreviewing => PreviewObject != null;
        public Func<TVisual, Vector3> GetOffset { get; set; } = x =>  Vector3.zero;
        
        public PreviewPlacer(Func<TVisual, Vector3> getPreviewPosition,Func<TObject, TVisual> getVisual,Action<TObject,TVisual> releaseVisual)
        {
            _getPreviewPosition = getPreviewPosition;
            _getVisual = getVisual;
            _releaseVisual = releaseVisual;
        }
        
        public void StartPreview(TObject module)
        {
            if (PreviewObject != null) StopPreview();
            
            _result = module;
            PreviewObject = _getVisual(module);
            
            if (PreviewObject == null) return;
            
            UpdatePreviewPosition();
        }

        public void UpdatePreviewPosition()
        {
            if (PreviewObject == null) return;
            
            Vector3 offset = GetOffset(PreviewObject);
            
            PreviewObject.Transform.Position = _getPreviewPosition(PreviewObject) - offset;
        }

        public TObject FinalizePreview()
        {
            if (PreviewObject == null) return default;
            
            TObject result = _result;
            
            StopPreview();
            
            return result;
        }

        public void StopPreview()
        {
            if (PreviewObject == null) return;
            
            _releaseVisual?.Invoke(_result, PreviewObject);
            PreviewObject = default;
            _result = default;
        }
    }
}