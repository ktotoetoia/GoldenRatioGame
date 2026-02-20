using UnityEngine;

namespace IM.Visuals
{
    public class AnimatorFloatParameterSetter : MonoBehaviour
    {
        [SerializeField] private string _parameterName = "CycleOffset";
        private Animator _animator;
        private float _value;
        
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                _animator.SetFloat(_parameterName, _value);
            }
        }

        private void Awake()
        {
            if (!TryGetComponent(out _animator)) throw new MissingComponentException(nameof(Animator));
        }
    }
}