using System;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class CappedValueElement : VisualElement
    {
        private const string UssProgressBar = "cappedValueProgressBar";
        private const string UssCappedValueElement = "cappedValueElement";
        private readonly ProgressBar _valueBar;
        
        public Func<ICappedValueReadOnly<float>> GetCappedValue { get; set; }

        private ICappedValueReadOnly<float> _lastValue;
        
        public CappedValueElement()
        {
            _valueBar = new ProgressBar();
            Add(_valueBar);
            _valueBar.AddToClassList(UssProgressBar);
            AddToClassList(UssCappedValueElement);

            _valueBar.RegisterCallback<GeometryChangedEvent>(_ => ApplyValues());
        }
        
        private void ApplyValues()
        {
            if (_lastValue == null) return;
            
            _valueBar.lowValue = _lastValue.MinValue;
            _valueBar.highValue = _lastValue.MaxValue;
            _valueBar.value = _lastValue.Value;
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