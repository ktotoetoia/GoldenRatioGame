using System;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class CappedValueElement : VisualElement
    {
        private const string UssClassName   = "cappedValueElement";
        private const string UssProgressBar = "cappedValueProgressBar";
        private readonly ProgressBar _valueBar;
        private readonly Label _valueLabel;
 
        public Func<ICappedValueReadOnly<float>> GetCappedValue { get; set; }
 
        private ICappedValueReadOnly<float> _lastValue;
 
        [UxmlAttribute] public bool   ShowLabel  { get; set; } = true;
        [UxmlAttribute] public string Separator  { get; set; } = "/";
        [UxmlAttribute] public string ValueFormat { get; set; } = "0";
 
        public CappedValueElement()
        {
            AddToClassList(UssClassName);

            _valueBar = new ProgressBar();
            _valueBar.AddToClassList(UssProgressBar);
            Add(_valueBar);

            _valueLabel = _valueBar.Q<Label>();
    
            _valueBar.RegisterCallback<GeometryChangedEvent>(_ => ApplyValues());
        }
 
        private void ApplyValues()
        {
            if (_lastValue == null) return;
 
            _valueBar.lowValue  = _lastValue.MinValue;
            _valueBar.highValue = _lastValue.MaxValue;
            _valueBar.value     = _lastValue.Value;
 
            _valueLabel.style.display = ShowLabel ? DisplayStyle.Flex : DisplayStyle.None;
 
            if (ShowLabel)
            {
                string current = _lastValue.Value.ToString(ValueFormat);
                string max     = _lastValue.MaxValue.ToString(ValueFormat);
                _valueLabel.text = $"{current}{Separator}{max}";
            }
        }
 
        public void Update()
        {
            ICappedValueReadOnly<float> value = GetCappedValue?.Invoke();
            if (value == null) return;
 
            _lastValue = value;
            ApplyValues();
        }
    }
}