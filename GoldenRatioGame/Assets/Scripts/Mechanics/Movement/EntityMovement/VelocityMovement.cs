using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(IVelocityModifier))]
    public class VelocityMovement : MonoBehaviour, IVectorMovement
    {
        [SerializeField] private float _rawSpeed;
        private IVelocityModifier _modifier;

        public ISpeed Speed { get; private set; }
        public Vector2 CurrentMovementDirection { get; private set; }

        private void Awake()
        {
            Speed = new Speed(_rawSpeed);
            _modifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            Speed.RawValue = _rawSpeed;
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, CurrentMovementDirection * Speed.FinalValue));
        }

        public void Move(Vector2 direction)
        {
            CurrentMovementDirection = direction;
        }
    }
}