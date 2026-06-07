using System;
using IM.Values;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class GhostCappedValueElement : VisualElement
    {
        private const string UssClassName       = "cappedValueElement";
        private const string UssBackground      = "cappedValueBackground";
        private const string UssGhostBar        = "cappedValueGhost";
        private const string UssHpBar           = "cappedValueHp";

        private const float GhostDelaySeconds  = 0.5f;
        private const float GhostSlideDuration = 0.4f;

        private readonly VisualElement _background;
        private readonly VisualElement _ghostBar;
        private readonly VisualElement _hpBar;

        public Func<ICappedValueReadOnly<float>> GetCappedValue { get; set; }

        private ICappedValueReadOnly<float> _lastValue;

        private float _ghostNorm   = 1f;
        private float _currentNorm = 1f;
        private float _delayTimer  = 0f;
        private bool  _waiting     = false;
        private bool  _sliding     = false;
        private float _slideTimer  = 0f;

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

        public void Update()
        {
            ICappedValueReadOnly<float> value = GetCappedValue?.Invoke();
            if (value == null) return;

            _lastValue = value;
            float newNorm = CalcNorm();

            if (newNorm < _currentNorm)
            {
                _waiting     = true;
                _sliding     = false;
                _delayTimer  = GhostDelaySeconds;
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
                    _waiting     = false;
                    _sliding     = true;
                    _slideTimer  = GhostSlideDuration;
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