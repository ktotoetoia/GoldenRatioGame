using System.Collections.Generic;
using IM.Augments;
using IM.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ModuleEntityAugmentPreviewer : MonoBehaviour, IModuleEntityStatPreviewer
    {
        [SerializeField] private List<StyleSheet> _iconStyleSheets;
        
        public VisualElement GetPreview(IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            var container = new AugmentScrollView();
            
            container.IconStyleSheets.AddRange(_iconStyleSheets);
            UpdatePreview(container, entity, currentContext);
        
            return container;
        }

        public void UpdatePreview(VisualElement previewElement, IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            if (previewElement is AugmentScrollView augmentScrollView)
            {
                if (entity.GameObject.TryGetComponent(out IAugmentContainer augmentContainer))
                {
                    augmentScrollView.SetAugments(augmentContainer.Augments);
                }
                else
                {
                    augmentScrollView.SetAugments(null); 
                }
            }
        }
    }
}