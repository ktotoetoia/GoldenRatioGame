using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IM.Effects;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class HealthPreviewer : MonoBehaviour, IStatPreviewer
    {
        [SerializeField] private List<StyleSheet> _styleSheets;

        private readonly ConditionalWeakTable<VisualElement, List<CappedValueElement>> _cappedValueCache = new();

        public VisualElement GetPreview(object item)
        {
            if (item is MonoBehaviour go && go.TryGetComponent(out IEffectGroupExtension extension) && extension.EffectGroup.Modifiers.TryGetAll(out IEnumerable<IHealthEffectModifier> effectModifiers))
            {
                VisualElement ele = new VisualElement();
                var cappedValues = new List<CappedValueElement>();

                foreach (IHealthEffectModifier effectModifier in effectModifiers)
                {
                    var a = new CappedValueElement
                    {
                        GetCappedValue = () => effectModifier.Health
                    };

                    ele.Add(a);
                    cappedValues.Add(a);
                }

                foreach (StyleSheet styleSheet in _styleSheets)
                {
                    ele.styleSheets.Add(styleSheet);
                }

                _cappedValueCache.Add(ele, cappedValues);

                return ele;
            }

            return null;
        }

        public void UpdatePreview(VisualElement previewElement, object item)
        {
            if (_cappedValueCache.TryGetValue(previewElement, out List<CappedValueElement> cappedValues))
            {
                foreach (CappedValueElement cappedValueElement in cappedValues) cappedValueElement.Update();
            }
        }
    }
}