using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    public class CurveMovement : IVectorMovement
    {
        private readonly IVelocityModifier _modifier;
        private readonly AnimationCurve _curve;
        
        public Accelerator Accelerator { get; }
        public float AccelerationTime
        {
            get => Accelerator.AccelerationTime;
            set => Accelerator.AccelerationTime = value;
        }
        public ISpeed Speed { get; private set; }
        public Vector2 Direction { get; set; }
        public Vector2 MovementVelocity { get; private set; }
        
        public CurveMovement(IVelocityModifier velocityModifier, AnimationCurve curve, ISpeed speed)
        {
            _modifier = velocityModifier;
            _curve = curve;
            Speed = speed;
            Accelerator = new Accelerator();
        }
        
        public void Apply()
        {
            Accelerator.Update(Direction,Time.fixedDeltaTime);
            
            Vector2 acceleration = Accelerator.Acceleration;
            Vector2 value = new Vector2(_curve.Evaluate(acceleration.x), _curve.Evaluate(acceleration.y)) *
                            new Vector2(acceleration.normalized.x, acceleration.normalized.y);

            MovementVelocity = value * Speed.FinalValue;
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, MovementVelocity));
        }
    }
}