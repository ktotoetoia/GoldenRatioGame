using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class GhostCappedValueElement : CappedValueElementBase
    {
        private const string UssGhostBar = UssClassName + "__ghost-bar";

        private readonly VisualElement _ghostBar;

        public float GhostDelaySeconds  { get; set; } = 0.5f;
        public float GhostSlideDuration { get; set; } = 0.4f;

        private float _ghostNorm   = 1f;
        private float _currentNorm = 1f;
        private float _delayTimer  = 0f;
        private bool  _waiting     = false;
        private bool  _sliding     = false;
        private float _slideTimer  = 0f;

        public GhostCappedValueElement()
        {
            _ghostBar = new VisualElement();
            _ghostBar.AddToClassList(UssGhostBar);
            _background.Insert(0, _ghostBar);
        }

        protected override void OnValueUpdated(float norm)
        {
            if (norm < _currentNorm)
            {
                _waiting    = true;
                _sliding    = false;
                _delayTimer = GhostDelaySeconds;
            }

            _currentNorm = norm;
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