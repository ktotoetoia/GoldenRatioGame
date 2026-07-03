using System;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public abstract partial class CappedValueElementBase : VisualElement
    {
        protected const string UssClassName        = "capped-value";
        protected const string UssBackground       = UssClassName + "__background";
        protected const string UssHpBar            = UssClassName + "__hp-bar";
        protected const string UssDividerContainer = UssClassName + "__dividers";
        protected const string UssDivider          = UssClassName + "__divider";
        protected const string UssDividerMinor     = UssDivider + "--minor";
        protected const string UssDividerMajor     = UssDivider + "--major";
        protected const string UssForeground       = UssClassName + "__foreground";
        protected const string UssLabel            = UssClassName + "__label";

        protected readonly VisualElement _background;
        protected readonly VisualElement _hpBar;
        protected readonly VisualElement _dividersContainer;
        protected readonly VisualElement _foreground;
        protected readonly Label _label;

        public Func<ICappedValueReadOnly<float>> GetCappedValue { get; set; }

        [UxmlAttribute] public int MinorDividerInterval { get; set; } = 100;
        [UxmlAttribute] public int MajorDividerInterval { get; set; } = 1000;

        [UxmlAttribute] public bool   ShowLabel   { get; set; } = true;
        [UxmlAttribute] public string Separator   { get; set; } = "/";
        [UxmlAttribute] public string ValueFormat { get; set; } = "0";

        protected ICappedValueReadOnly<float> _lastValue;

        private float _lastKnownMax = -1f;
        private float _lastKnownMin = -1f;

        protected CappedValueElementBase()
        {
            AddToClassList(UssClassName);

            _background = new VisualElement();
            _background.AddToClassList(UssBackground);
            Add(_background);

            _hpBar = new VisualElement();
            _hpBar.AddToClassList(UssHpBar);
            _background.Add(_hpBar);

            _dividersContainer = new VisualElement();
            _dividersContainer.AddToClassList(UssDividerContainer);
            _background.Add(_dividersContainer);

            _foreground = new VisualElement();
            _foreground.AddToClassList(UssForeground);
            _foreground.pickingMode = PickingMode.Ignore;
            Add(_foreground);

            _label = new Label();
            _label.AddToClassList(UssLabel);
            _label.pickingMode = PickingMode.Ignore;
            Add(_label);
        }

        protected float CalcNorm()
        {
            if (_lastValue == null) return 1f;
            float range = _lastValue.MaxValue - _lastValue.MinValue;
            return range > 0f
                ? (_lastValue.Value - _lastValue.MinValue) / range
                : 1f;
        }

        protected static void SetBarWidth(VisualElement bar, float norm)
        {
            bar.style.width = new StyleLength(new Length(Mathf.Clamp01(norm) * 100f, LengthUnit.Percent));
        }

        private void RebuildDividersIfNeeded()
        {
            if (Mathf.Approximately(_lastValue.MaxValue, _lastKnownMax) &&
                Mathf.Approximately(_lastValue.MinValue, _lastKnownMin))
                return;

            _lastKnownMax = _lastValue.MaxValue;
            _lastKnownMin = _lastValue.MinValue;

            _dividersContainer.Clear();

            float range = _lastKnownMax - _lastKnownMin;
            if (range <= 0f || MinorDividerInterval <= 0) return;

            for (int i = 1; i * MinorDividerInterval < range; i++)
            {
                int offset = i * MinorDividerInterval;
                bool isMajor = MajorDividerInterval > 0 && offset % MajorDividerInterval == 0;

                var divider = new VisualElement();
                divider.AddToClassList(isMajor ? UssDividerMajor : UssDividerMinor);
                divider.style.left = new StyleLength(new Length(offset / range * 100f, LengthUnit.Percent));
                _dividersContainer.Add(divider);
            }
        }

        private void ApplyLabel()
        {
            _label.style.display = ShowLabel ? DisplayStyle.Flex : DisplayStyle.None;
            if (!ShowLabel) return;

            string current = _lastValue.Value.ToString(ValueFormat);
            string max      = _lastValue.MaxValue.ToString(ValueFormat);
            _label.text = $"{current}{Separator}{max}";
        }

        public virtual void Update()
        {
            ICappedValueReadOnly<float> value = GetCappedValue?.Invoke();
            if (value == null) return;

            _lastValue = value;

            RebuildDividersIfNeeded();
            ApplyLabel();

            float norm = CalcNorm();
            SetBarWidth(_hpBar, norm);
            OnValueUpdated(norm);
        }

        protected virtual void OnValueUpdated(float norm) { }
    }
}