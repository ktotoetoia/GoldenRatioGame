using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IM.Augments;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class AugmentsProgressionPreviewer : MonoBehaviour, IAugmentPreviewer
    {
        [SerializeField] private List<StyleSheet> _styleSheets;

        public VisualElement GetPreview(object item)
        {
            if (item is IExtensibleItem ite && ite.Extensions.TryGet(out IAugmentSource augmentSource) && ite.Owner is IEntity entity)
            {
                var element = new AugmentSourceInfoElement();

                foreach (StyleSheet styleSheet in _styleSheets) 
                    element.styleSheets.Add(styleSheet);

                element.SetSource(augmentSource.GetFor(entity));

                return element;
            }

            return null;
        }

        public void UpdatePreview(VisualElement previewElement, object item)
        {
            if (previewElement is not AugmentSourceInfoElement element) return;

            if (item is IExtensibleItem ite && ite.Extensions.TryGet(out IAugmentSource augmentSource) && ite.Owner is IEntity entity)
            {
                element.SetSource(augmentSource.GetFor(entity));
            }
        }
    }
}