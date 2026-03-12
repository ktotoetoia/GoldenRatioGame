using UnityEngine;

namespace IM.Visuals
{
    public class AnimatorBoolParameterSetter : MonoBehaviour
    {
        [SerializeField] private string _parameterName;
        private bool _value;
        private Animator _animator;
        
        public bool Value
        {
            get => (_animator??= GetComponent<Animator>()).GetBool(_parameterName);
            set => (_animator??= GetComponent<Animator>()).SetBool(_parameterName, value);
        }
    }
}