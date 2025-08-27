using IM.Economy;
using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(IVelocityModifier))]
    public class VelocityMovement : MonoBehaviour, IVectorMovement
    {
        [SerializeField] private Speed _speed;
        private IVelocityModifier _modifier;
        private Vector2 _direction;

        public ISpeed Speed => _speed;

        private void Awake()
        {
            _modifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, _direction * Speed.FinalValue));
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
        }
    }
}