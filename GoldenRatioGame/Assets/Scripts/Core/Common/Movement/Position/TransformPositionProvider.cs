using UnityEngine;

namespace IM.Common
{
    public class TransformPositionProvider : MonoBehaviour, IPositionProvider
    {
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}