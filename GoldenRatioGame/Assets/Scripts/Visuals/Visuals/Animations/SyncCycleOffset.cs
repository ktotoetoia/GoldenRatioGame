using UnityEngine;

namespace IM.Visuals
{
    public class SyncCycleOffset : StateMachineBehaviour
    {
        [SerializeField] private string offsetParameterName;
        private float _lastOffsetValue;
        private bool _isInitialized;

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float currentParameterValue = animator.GetFloat(offsetParameterName);

            if (!_isInitialized)
            {
                _lastOffsetValue = currentParameterValue;
                _isInitialized = true;
                return;
            }

            if (!Mathf.Approximately(currentParameterValue, _lastOffsetValue))
            {
                ApplyOffsetShift(animator, stateInfo, layerIndex, currentParameterValue);
                _lastOffsetValue = currentParameterValue;
            }
        }

        private void ApplyOffsetShift(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, float newOffset)
        {
            float delta = newOffset - _lastOffsetValue;

            float newNormalizedTime = stateInfo.normalizedTime + delta;

            animator.Play(stateInfo.fullPathHash, layerIndex, newNormalizedTime);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isInitialized = false;
        }
    }
}