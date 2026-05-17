using UnityEngine;

namespace IM.Visuals
{
    public class TriggerParameterOnEnable : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _parameterName;

        private void OnEnable()
        {
            _animator.SetTrigger(_parameterName);
        }
    }
}