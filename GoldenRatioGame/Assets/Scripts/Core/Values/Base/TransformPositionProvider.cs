using UnityEngine;

namespace IM.Values
{
    public class TransformPositionProvider : MonoBehaviour, IPositionProvider
    {
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}