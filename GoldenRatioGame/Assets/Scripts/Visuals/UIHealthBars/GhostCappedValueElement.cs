using System;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{

    [UxmlElement]
    public partial class GhostCappedValueElement : VisualElement
    {
        private const string UssClassName        = "cappedValueElement";
        private const string UssBackground       = "cappedValueBackground";
        private const string UssGhostBar         = "cappedValueGhost";
        private const string UssHpBar            = "cappedValueHp";
        private const string UssDividerContainer = "cappedValueDividers";
        private const string UssDividerMinor     = "cappedValueDividerMinor";
        private const string UssDividerMajor     = "cappedValueDividerMajor";

        private readonly VisualElement _background;
        private readonly VisualElement _ghostBar;
        private readonly VisualElement _hpBar;
        private readonly VisualElement _dividersContainer;

        public Func<ICappedValueReadOnly<float>> GetCappedValue { get; set; }

        [UxmlAttribute] public int MinorDividerInterval { get; set; } = 100;
        [UxmlAttribute] public int MajorDividerInterval { get; set; } = 1000;
        public float GhostDelaySeconds { get; set; } = 0.5f;
        public float GhostSlideDuration { get; set; } = 0.4f;

        private ICappedValueReadOnly<float> _lastValue;

        private float _ghostNorm   = 1f;
        private float _currentNorm = 1f;
        private float _delayTimer  = 0f;
        private bool  _waiting     = false;
        private bool  _sliding     = false;
        private float _slideTimer  = 0f;

        private float _lastKnownMax = -1f;
        private float _lastKnownMin = -1f;

        public GhostCappedValueElement()
        {
            AddToClassList(UssClassName);

            _background = new VisualElement();
            _background.AddToClassList(UssBackground);
            Add(_background);

            _ghostBar = new VisualElement();
            _ghostBar.AddToClassList(UssGhostBar);
            _background.Add(_ghostBar);

            _hpBar = new VisualElement();
            _hpBar.AddToClassList(UssHpBar);
            _background.Add(_hpBar);

            _dividersContainer = new VisualElement();
            _dividersContainer.AddToClassList(UssDividerContainer);
            _background.Add(_dividersContainer);
        }

        private float CalcNorm()
        {
            if (_lastValue == null) return 1f;
            float range = _lastValue.MaxValue - _lastValue.MinValue;
            return range > 0f
                ? (_lastValue.Value - _lastValue.MinValue) / range
                : 1f;
        }

        private void SetBarWidth(VisualElement bar, float norm)
        {
            bar.style.width = new StyleLength(
                new Length(Mathf.Clamp01(norm) * 100f, LengthUnit.Percent));
        }

        private void RebuildDividers()
        {
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

        public void Update()
        {
            ICappedValueReadOnly<float> value = GetCappedValue?.Invoke();
            if (value == null) return;

            _lastValue = value;

            if (!Mathf.Approximately(_lastValue.MaxValue, _lastKnownMax) || !Mathf.Approximately(_lastValue.MinValue, _lastKnownMin))
            {
                _lastKnownMax = _lastValue.MaxValue;
                _lastKnownMin = _lastValue.MinValue;
                RebuildDividers();
            }

            float newNorm = CalcNorm();

            if (newNorm < _currentNorm)
            {
                _waiting    = true;
                _sliding    = false;
                _delayTimer = GhostDelaySeconds;
            }

            _currentNorm = newNorm;
            SetBarWidth(_hpBar, _currentNorm);
            SetBarWidth(_ghostBar, _ghostNorm);
        }

        public void Tick(float deltaTime)
        {
            if (!_waiting && !_sliding) return;

            if (_waiting)
            {
                _delayTimer -= deltaTime;
                if (_delayTimer <= 0f)
                {
                    _waiting    = false;
                    _sliding    = true;
                    _slideTimer = GhostSlideDuration;
                }
            }

            if (_sliding)
            {
                _slideTimer -= deltaTime;

                float t     = 1f - Mathf.Clamp01(_slideTimer / GhostSlideDuration);
                float eased = 1f - (1f - t) * (1f - t);
                _ghostNorm  = Mathf.Lerp(_ghostNorm, _currentNorm, eased);

                SetBarWidth(_ghostBar, _ghostNorm);

                if (_slideTimer <= 0f)
                {
                    _sliding   = false;
                    _ghostNorm = _currentNorm;
                    SetBarWidth(_ghostBar, _ghostNorm);
                }
            }
        }
    }
}