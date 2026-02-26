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
        public Vector2 Direction { get; set; }
        public Vector2 MovementVelocity { get; private set; }

        private void Awake()
        {
            Speed = new Speed(_rawSpeed);
            _modifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            Speed.RawValue = _rawSpeed;
            MovementVelocity = Direction * Speed.FinalValue;
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, MovementVelocity));
        }
    }
}