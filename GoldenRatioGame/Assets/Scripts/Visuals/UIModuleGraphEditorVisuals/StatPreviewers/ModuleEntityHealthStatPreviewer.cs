using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using IM.Effects;
using IM.Graphs;
using IM.Modules;
using IM.UI;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ModuleEntityHealthStatPreviewer : MonoBehaviour, IModuleEntityStatPreviewer
    {
        [SerializeField] private List<StyleSheet> _requiredStyles;

        public VisualElement GetPreview(IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            var previewElement = new CappedValueElement();
            foreach (var styleSheet in _requiredStyles) previewElement.styleSheets.Add(styleSheet);
            previewElement.GetCappedValue = () => CalculateTotalHealth(entity, currentContext);
            UpdatePreview(previewElement, entity, currentContext);
        
            return previewElement;
        }

        public void UpdatePreview(VisualElement previewElement, IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            if (previewElement is not CappedValueElement healthDisplay) return;
            
            healthDisplay.GetCappedValue = () => CalculateTotalHealth(entity, currentContext);
            healthDisplay.Update();
        }

        private CappedValue<float> CalculateTotalHealth(IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            float totalValue = 0f;
            float totalMax = 0f;

            if (currentContext?.Graph?.DataModules != null)
            {
                foreach (IDataModule<IExtensibleItem> module in currentContext.Graph.DataModules)
                {
                    if (module?.Value?.Extensions.TryGet(out IEffectGroupExtension e) == true &&
                        e.EffectGroup?.Modifiers?.TryGetAll(out IEnumerable<IHealthEffectModifier> effectModifiers) == true)
                    {
                        Accumulate(effectModifiers, ref totalValue, ref totalMax);
                    }
                }
            }

            var augmentContainer = entity?.GameObject?.GetComponent<IAugmentContainer>();
            if (augmentContainer?.Augments != null)
            {
                foreach (IEffectAugment effectAugment in augmentContainer.Augments.OfType<IEffectAugment>())
                {
                    if (effectAugment.EffectGroup?.Modifiers?.TryGetAll(out IEnumerable<IHealthEffectModifier> effectModifiers) == true)
                    {
                        Accumulate(effectModifiers, ref totalValue, ref totalMax);
                    }
                }
            }
            return new CappedValue<float>(0,totalMax,totalValue);
        }

        private void Accumulate(IEnumerable<IHealthEffectModifier> modifiers, ref float value, ref float max)
        {
            if (modifiers == null) return;

            foreach (IHealthEffectModifier modifier in modifiers)
            {
                if (modifier?.Health != null)
                {
                    value += modifier.Health.Value;
                    max += modifier.Health.MaxValue;
                }
            }
        }
    }
}