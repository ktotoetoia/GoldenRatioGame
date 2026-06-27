using System.Collections.Generic;
using IM.Effects;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class HealthPreviewer : MonoBehaviour, IStatPreviewer
    {
        [SerializeField] private List<StyleSheet>  _styleSheets;
        
        public VisualElement GetPreview(object item)
        {
            if (item is MonoBehaviour go && go.TryGetComponent(out IEffectGroupExtension extension) && extension.EffectGroup.Modifiers.TryGetAll(out IEnumerable<IHealthEffectModifier> effectModifiers))
            {
                VisualElement ele = new VisualElement();

                foreach (IHealthEffectModifier effectModifier in effectModifiers)
                {
                    var a = new CappedValueElement
                    {
                        GetCappedValue = () => effectModifier.Health
                    };

                    ele.Add(a);
                }

                foreach (StyleSheet styleSheet in _styleSheets)
                {
                    ele.styleSheets.Add(styleSheet);
                }

                return ele;
            }

            return null;
        }
        
        public void UpdatePreview(VisualElement previewElement, object item)
        {
            foreach (CappedValueElement cappedValueElement in previewElement.Query<CappedValueElement>().ToList())
            {
                cappedValueElement.Update();
            }
        }
    }
}