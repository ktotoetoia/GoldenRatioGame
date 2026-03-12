using IM.Common;
using UnityEngine;

namespace IM.Movement
{
    public interface IVectorMovement : IMoveInVector, IHaveSpeed
    {
        Vector2 MovementVelocity { get; }
    }
}