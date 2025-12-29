using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    public interface IVectorMovement : IMoveInVector, IHaveSpeed
    {
        Vector2 MovementVelocity { get; }
        Vector2 MovementDirection { get; }
    }
}