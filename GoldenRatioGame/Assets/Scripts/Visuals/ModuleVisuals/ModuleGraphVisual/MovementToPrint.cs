using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementToPrint : MonoBehaviour, IRequireMovement
    {
        public void UpdateCurrentVelocity(Vector2 velocity)
        {
            if(velocity == Vector2.zero) Debug.Log("Stand");
        }
    }
}