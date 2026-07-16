using System.Collections.Generic;
using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class AugmentProgressRowPreviewer : MonoBehaviour, IStatPreviewer
    {
        [SerializeField] private List<StyleSheet> _styleSheets = new();
        
        public VisualElement GetPreview(object item)
        {
            if (item is not IAugmentProgress progress) return null;

            var element = new AugmentProgressRowElement();
            element.SetProgress(progress);
            
            foreach (StyleSheet styleSheet in _styleSheets) element.styleSheets.Add(styleSheet);
            
            return element;
        }

        public void UpdatePreview(VisualElement previewElement, object item)
        {
            if (previewElement is not AugmentProgressRowElement element) return;
            if (item is not IAugmentProgress progress) return;

            element.SetProgress(progress);
        }
    }
}