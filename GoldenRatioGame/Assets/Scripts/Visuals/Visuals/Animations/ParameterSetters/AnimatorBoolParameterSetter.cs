using UnityEngine;

namespace IM.Visuals
{
    public class AnimatorBoolParameterSetter : MonoBehaviour
    {
        [SerializeField] private string _parameterName = "CycleOffset";
        private Animator _animator;
        private bool _value;
        
        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                _animator.SetBool(_parameterName, _value);
            }
        }

        private void Awake()
        {
            if (!TryGetComponent(out _animator)) throw new MissingComponentException(nameof(Animator));
        }

        private void OnEnable()
        {
            _animator.SetBool(_parameterName, _value);
        }
    }
}