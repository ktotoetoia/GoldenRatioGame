using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class TransformPositionProvider : MonoBehaviour, IPositionProvider
    {
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}